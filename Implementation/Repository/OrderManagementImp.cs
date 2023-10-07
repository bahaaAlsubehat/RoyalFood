using Interface.DTO;
using Interface.IRepository;
using Interface.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Repository
{
    public class OrderManagementImp: IOrderManagement
    {
        private readonly RoyalFoodContext _context;
        private readonly ILogger<OrderManagementImp> _logger;
        public OrderManagementImp(RoyalFoodContext cotext, ILogger<OrderManagementImp> logger) 
        {
            _context = cotext;
            _logger = logger;
        
        }

        public async Task<string> CreateCart(int userid, int? itemid, int? mealid, int qnty)
        {
            var cart = await _context.Carts.Where(x=>x.UserId ==userid && x.IsActive == true).SingleOrDefaultAsync();
            if (cart != null)
            {
                var item = await _context.Items.Where(x=>x.ItemId == itemid).SingleOrDefaultAsync();
                var meal = await _context.Meals.Where(x=>x.MealId == mealid).SingleOrDefaultAsync();
                if(item != null)
                {
                    var cartitemmeal1 = await _context.CartItemMeals.Where(x => x.CartId == cart.CartId && x.ItemId == itemid).SingleOrDefaultAsync();
                    if(cartitemmeal1 == null) 
                    {
                        CartItemMeal cartitemmeal= new()
                        {
                            ItemId = itemid,
                            Quantity = qnty,
                            NetPrice =item.Price * qnty,
                            CartId = cart.CartId
                        };
                        _logger.LogInformation("Added Item to Cart");
                        await _context.AddAsync(cartitemmeal);
                        return "Added Item Successfully";
                       
                    }
                    else
                    {
                        cartitemmeal1.Quantity += qnty;
                        cartitemmeal1.NetPrice = item.Price * qnty;
                        _context.Update(cartitemmeal1);
                        return "Updated on Quantity Item Successfully";

                    }

                }
                var cartitemmeal2 = await _context.CartItemMeals.Where(x=>x.CartId == cart.CartId && x.MealId == mealid).SingleOrDefaultAsync();
                if(meal != null)
                {
                    if(cartitemmeal2 == null)
                    {
                        CartItemMeal cartitemmeal = new()
                        {
                            MealId = mealid,
                            Quantity = qnty,
                            NetPrice = meal.Price * qnty,
                            CartId = cart.CartId
                        };
                        _logger.LogInformation("Added Meal to Cart");
                        await _context.AddAsync(cartitemmeal);
                        return "Added Meal Successfully";
                    }
                    else
                    {
                        cartitemmeal2.Quantity += qnty;
                        cartitemmeal2.NetPrice = meal.Price * qnty;
                        _context.Update(cartitemmeal2);
                        return "Updated on Quantity Meal Successfully";
                    }
                }
                return "No Meal or Item Added";

            }
            else
            {
                var user = await _context.Users.Where(x => x.UserId == userid).SingleOrDefaultAsync();
                if(user != null) 
                {
                    Cart cartn = new()
                    {
                        UserId = user.UserId,
                        IsActive = true
                    };
                    await _context.AddAsync(cartn);
                    await _context.SaveChangesAsync();
                    
                    var cartnew = await _context.Carts.Where(x=>x.UserId == userid).SingleOrDefaultAsync();
                    var itemn = await _context.Items.Where(x=>x.ItemId == itemid).SingleOrDefaultAsync();
                    var mealn = await _context.Meals.Where(x=>x.MealId == mealid).SingleOrDefaultAsync();
                    if(itemid != null)
                    {
                        var cartitemm = await _context.CartItemMeals.Where(x => x.ItemId == itemid && x.CartId == cartnew.CartId).SingleOrDefaultAsync();
                        if(cartitemm == null)
                        {
                            CartItemMeal cartitemmeal = new()
                            {
                                ItemId = itemid,
                                Quantity = qnty,
                                NetPrice = itemn.Price * qnty,
                                CartId = cart.CartId
                            };
                            _logger.LogInformation("Added a new Item in Cart");
                            await _context.AddAsync(cartitemmeal);
                            return "Added a new Item in Cart";
                        }
                        else
                        {
                            cartitemm.Quantity += qnty;
                            cartitemm.NetPrice = itemn.Price * qnty;
                            _context.Update(cartitemm);
                            return "Updated on Quantity Item Successfully";
                        }
                        

                    }
                    else
                    if(mealid  != null) 
                    {
                        var cartimeal = await _context.CartItemMeals.Where(x => x.MealId == mealid && x.CartId == cartnew.CartId).SingleOrDefaultAsync();
                        if(cartimeal == null)
                        {
                            CartItemMeal cartitemmeal = new()
                            {
                                ItemId = mealid,
                                Quantity = qnty,
                                NetPrice = mealn.Price * qnty,
                                CartId = cart.CartId
                            };
                            _logger.LogInformation("Added Meal to Cart");
                            await _context.AddAsync(cartitemmeal);
                            return "Added Meal Successfully";
                        }
                        else
                        {
                            cartimeal.Quantity += qnty;
                            cartimeal.NetPrice = mealn.Price * qnty;
                            _context.Update(cartimeal);
                            return "Updated on Quantity Meal Successfully";
                        }
                       

                    }



                }
                return "No Meal or Item Added";
            }

        }
        public async Task<string> DeleteFromCart(int cartitml)
        {
            try
            {
                var cartitemmmeal = await _context.CartItemMeals.Where(x=>x.CartItemMealId == cartitml).SingleOrDefaultAsync();
                if(cartitemmmeal != null) 
                {
                    var cart = await _context.Carts.Where(x=>x.CartId == cartitemmmeal.CartId).SingleOrDefaultAsync();
                    if(cart != null) 
                    { 
                        if(cartitemmmeal.Quantity == 1)
                        {
                            _context.Remove(cartitemmmeal);
                            _logger.LogInformation("You Removed it from Cart");
                            return "You Removed it from Cart";
                        }
                        else
                        {
                            cartitemmmeal.Quantity -= 1;
                            _context.Update(cartitemmmeal);
                            _logger.LogInformation("You Removed quantity it from Cart");
                            return "You Removed quantity it from Cart";


                        }

                    }
                }
                return string.Empty;
            }
            catch (Exception ex) 
            {
                _logger.LogError("We Get Exception");
                return ex.Message;
            
            }
        }
        public async Task<string> AddNewOrder(OrderDTO orderDTO)
        {
            try
            {
                var cart =  await _context.Carts.Where(x=>x.CartId == orderDTO.cartid && x.IsActive == true).SingleOrDefaultAsync();
                if (cart != null )
                {
                    cart.IsActive = false;
                    _context.Update(cart);
                    _context.SaveChanges();

                    Order order = new()
                    {
                        CartId = orderDTO.cartid,
                        OrderDate = DateTime.UtcNow,
                        TotalPrice =  _context.CartItemMeals.Where(x=>x.CartId == orderDTO.cartid).Sum(x=>x.NetPrice),
                        OrderStatusId = 1,
                        DelivaryAddress = orderDTO.deliveryaddress,
                        DeliveryDate = DateTime.UtcNow.AddHours(0.5-2),
                        CustomerNotes = orderDTO.customernote,
                        PaymentId = orderDTO.paymentid,
                    };
                    if(order.OrderStatusId == 4) { order.RatingandFeedback = orderDTO.ratingfeedback; } 
                    await _context.AddAsync(order);
                    _logger.LogInformation("Create A new Order");
                    return "Successfully Creating Order";
                }
                return "Empty Cart return to Menu Page to Add Items or Meals";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ex.Message;
            }
        }
        public async Task<List<OrderResponseDTO>> Allorders()
        {
            try
            {
                var orderall = await _context.Orders.ToListAsync();
                if (orderall != null)
                {
                    var cart = await _context.Carts.Where(x=>x.IsActive == false).ToListAsync();
                    var cartmeal = await _context.CartItemMeals.ToListAsync();
                    var item = await _context.Items.ToListAsync();
                    var meal =await _context.Meals.ToListAsync();

                    var result = from c in cart
                                 join cim in cartmeal on c.CartId equals cim.CartId
                                 where cim.ItemId != null
                                 join i in item on cim.ItemId equals i.ItemId into ilist from i in ilist.DefaultIfEmpty()
                                 where ilist != null
                                 join m in meal on cim.MealId equals m.MealId into mlist from m in mlist.DefaultIfEmpty()
                                 where mlist != null
                                 //where c.CartId == cim.CartId
                                 join o in orderall on c.CartId equals o.CartId
                                 select new OrderItemandItemDTO
                                 {
                                     orderid = o.OrderId,
                                     itemid = cim?.ItemId,
                                     mealid = cim?.MealId,
                                     itemname = i?.ItemName, /*item.Where(x=>x.ItemId.Equals(cim?.ItemId)).First()?.ItemName,*/
                                     mealname = m?.MealName,  //meal.Where(x => x.MealId.Equals(cim?.MealId)).First()?.MealName,
                                     qnty = cim.Quantity.ToString(),
                                     netprice = cim.NetPrice.ToString()
                                 };


                    List < OrderResponseDTO > responseDTOs = new List<OrderResponseDTO>();
                    foreach (var order in orderall)
                    {
                        responseDTOs.Add(new OrderResponseDTO
                        {
                            ordernumber = order.OrderId.ToString(),
                            //customername = _context.Users.Where(x => x.UserId == cart.Single(y=>y.CartId == order.CartId).UserId).Single().FirstName,
                            //customerphone = _context.Users.Where(x => x.UserId == cart.Find(y=> y.CartId == order.CartId).UserId).SingleOrDefault().Phone,
                            orderDate = order.OrderDate.ToString(),
                            delivarydate = order.DeliveryDate.ToString(),
                            orderstatus = _context.OrderStatuses.Where(x => x.OrderStatusId == order.OrderStatusId).FirstOrDefault().Status,
                            totalprice = order.TotalPrice.ToString(),
                            orderitmlDTO = result?.Where(x=>x.orderid == order.OrderId).ToList() 
                        });
                    }
                    _logger.LogInformation("Show the list of Orders");
                    return responseDTOs;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return new List<OrderResponseDTO> { };

            }
        }


        #region OrderStatus
        public async Task<string> CreateOrderStatus(string OrderStatus)
        {
            try
            {
                OrderStatus orderstatus1 = new OrderStatus();
                if (OrderStatus == null && OrderStatus == orderstatus1.Status)
                {
                    return "Status is already Recorded or null value";
                }
                else
                {
                    orderstatus1.Status = OrderStatus;
                    await _context.AddAsync(orderstatus1);
                    _logger.LogInformation("Added New Status");
                    return "Added New Status";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return ex.Message;

            }
        }
        public async Task<string> EditeOrderStatus(int id, string status)
        {
            try
            {
                var ordersts = await _context.OrderStatuses.Where(x => x.OrderStatusId == id).SingleOrDefaultAsync();
                if (ordersts == null)
                {
                    _logger.LogWarning("Incorrect Id or null value");
                    return "Incorrect Id or null value";

                }
                else
                {
                    ordersts.Status = status;
                    _context.Update(ordersts);
                    _logger.LogInformation("Updated OrderStatus");
                    return ($"Updated on OrderStatus with Id Equal {id}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return ex.Message;
            }
        }

        public async Task<List<OrderStatus>> AllOrderStatus()
        {
            try
            {
                var orderstsall = await _context.OrderStatuses.ToArrayAsync();
                return orderstsall.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return new List<OrderStatus>();

            }
        }

        #endregion

        #region PaymentMethod
        public async Task<string> CreatePaymentMethod(string paymentway)
        {
            try
            {
                Payment payment = new()
                {
                    PaymentMethod = paymentway
                };
                await _context.AddAsync(payment);
                _logger.LogInformation("Added PaymentMethod Successfully");
                return "Added PaymentMethod Successfully";

            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error");
                return ex.Message;

            }
        }

        public async Task<string> EditePaymentMethod(int id, string paymentway)
        {
            try
            {
                var editepaymeth = await _context.Payments.Where(x => x.PaymentId == id).FirstOrDefaultAsync();
                if (editepaymeth != null)
                {
                    editepaymeth.PaymentMethod = paymentway;
                    _logger.LogInformation("Updated PAymentMethod Successfully");
                    _context.Update(editepaymeth);
                    return "Updated PAymentMethod Successfully";
                }
                else
                {
                    _logger.LogDebug("Null Value or incorrect Id");
                    return $"Null Value or incorrect Id {id}";
                }
            }
            catch (Exception ex) 
            {
                _logger.LogWarning("Error");
                return ex.Message;
            }
        }

        public async Task<List<Payment>> AllPayment()
        {
            try
            {
                var allpaymeth = await _context.Payments.ToListAsync();
                return allpaymeth;
            }
            catch (Exception ex) 
            { 
                _logger.LogWarning("Error");
                return new List<Payment>();
            }
        }

        #endregion



    }
}
