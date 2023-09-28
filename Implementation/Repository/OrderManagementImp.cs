using Interface.DTO;
using Interface.IRepository;
using Interface.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            var cart = await _context.Carts.Where(x=>x.UserId ==userid).SingleOrDefaultAsync();
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
