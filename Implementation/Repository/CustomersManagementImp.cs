using Interface.IRepository;
using Interface.Models;
using Microsoft.Extensions.Logging;
using Interface.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Repository
{
    public class CustomersManagementImp: ICustomersManagement
    {
        private readonly RoyalFoodContext _context;
        private readonly ILogger<CustomersManagementImp> _logger;
        public CustomersManagementImp(RoyalFoodContext context, ILogger<CustomersManagementImp> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CustomersResponseDTO>> AllCustomers()
        {
           
            try
            {
                var user = await _context.Users./*Where(x => x.RoleId == 7).*/ToListAsync();
                var customer = await _context.Customers.ToListAsync();
                var cart = await _context.Carts.ToListAsync();  
                var order = await _context.Orders.ToListAsync();
                var login =  await _context.Logins.ToListAsync();
                var result = from us in user
                             join c in cart on us.UserId equals c.UserId
                             join o in order on c.CartId equals o.CartId
                             where c.CartId == o.CartId
                             select new CustomerOrderDTO
                             {
                                 userid = us.UserId,
                                 ordernumber = o.OrderId.ToString(),
                                 orderdate = o.OrderDate?.ToString(),
                                 delivarydate = o.DeliveryDate?.ToString(),
                                 totalprice = o.TotalPrice?.ToString(),
                                 orderstatus =_context.OrderStatuses.Where(x => x.OrderStatusId == o.OrderStatusId).First().Status,

                             };

                List<CustomersResponseDTO?> response = new List<CustomersResponseDTO?>();
                foreach (var x in user)
                {
                    response.Add(new CustomersResponseDTO
                    {
                        customernamefirst = x.FirstName,
                        customernamelast = x.LastName,
                        username = _context.Customers.Where(x=>x.UserId == x.UserId).FirstOrDefault()?.UserName,
                        phone = x.Phone,
                        eamil = _context.Logins.Where(y => y.UserId == x.UserId).FirstOrDefault()?.Email,
                        address = x.Address,
                        ////lastlogin = _context.Logins.Where(x=>x.UserId == x.UserId).
                        lastorderdate = _context.Orders.OrderByDescending(f=>f.Cart.UserId == x.UserId).FirstOrDefault().OrderDate,
                        customerorderlist = result?.Where(y=>y.userid == x.UserId).ToList()


                    }); 
                }
                _logger.LogInformation("We Get All Customers");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogWarning("Get Exception");
                return null;
            }
        }

        public async Task<List<CustomersResponseDTO>> SortCustomer(int? CustomerId, string? FirstName, string? Phone, string? lastname)
        {
            try
            {
                //var user = await _context.Users.ToListAsync();
                if (CustomerId != null)
                {
                    var user = await _context.Users.Where(x => x.UserId == CustomerId).ToListAsync();
                    var customer = await _context.Customers.ToListAsync();
                    var cart = await _context.Carts.ToListAsync();
                    var order = await _context.Orders.ToListAsync();
                    var login = await _context.Logins.ToListAsync();
                    var result = from us in user
                                 join c in cart on us.UserId equals c.UserId
                                 join o in order on c.CartId equals o.CartId
                                 where c.CartId == o.CartId
                                 select new CustomerOrderDTO
                                 {
                                     userid = us.UserId,
                                     ordernumber = o.OrderId.ToString(),
                                     orderdate = o.OrderDate?.ToString(),
                                     delivarydate = o.DeliveryDate?.ToString(),
                                     totalprice = o.TotalPrice?.ToString(),
                                     orderstatus =/* o.OrderStatusId?.ToString() */_context.OrderStatuses.Where(x => x.OrderStatusId == o.OrderStatusId).First().Status,

                                 };

                    List<CustomersResponseDTO?> response = new List<CustomersResponseDTO?>();
                    foreach (var x in user)
                    {
                        response.Add(new CustomersResponseDTO
                        {
                            customernamefirst = x.FirstName,
                            customernamelast = x.LastName,
                            username = _context.Customers.Where(x => x.UserId == x.UserId).FirstOrDefault()?.UserName,
                            phone = x.Phone,
                            eamil = _context.Logins.Where(y => y.UserId == x.UserId).FirstOrDefault()?.Email,
                            address = x.Address,
                            ////lastlogin = _context.Logins.Where(x=>x.UserId == x.UserId).
                            lastorderdate = _context.Orders.OrderByDescending(f => f.Cart.UserId == x.UserId).FirstOrDefault().OrderDate,
                            customerorderlist = result?.Where(y => y.userid == x.UserId).ToList()


                        });
                    }
                    _logger.LogInformation("We Get All Customers");
                    return response;
                }
                if (FirstName != null)
                {
                    var  user = await _context.Users.Where(x => x.FirstName.Contains(FirstName)).ToListAsync();
                    var customer = await _context.Customers.ToListAsync();
                    var cart = await _context.Carts.ToListAsync();
                    var order = await _context.Orders.ToListAsync();
                    var login = await _context.Logins.ToListAsync();
                    var result = from us in user
                                 join c in cart on us.UserId equals c.UserId
                                 join o in order on c.CartId equals o.CartId
                                 where c.CartId == o.CartId
                                 select new CustomerOrderDTO
                                 {
                                     userid = us.UserId,
                                     ordernumber = o.OrderId.ToString(),
                                     orderdate = o.OrderDate?.ToString(),
                                     delivarydate = o.DeliveryDate?.ToString(),
                                     totalprice = o.TotalPrice?.ToString(),
                                     orderstatus = _context.OrderStatuses.Where(x => x.OrderStatusId == o.OrderStatusId).First().Status,

                                 };

                    List<CustomersResponseDTO?> response = new List<CustomersResponseDTO?>();
                    foreach (var x in user)
                    {
                        response.Add(new CustomersResponseDTO
                        {
                            customernamefirst = x.FirstName,
                            customernamelast = x.LastName,
                            username = _context.Customers.Where(x => x.UserId == x.UserId).FirstOrDefault()?.UserName,
                            phone = x.Phone,
                            eamil = _context.Logins.Where(y => y.UserId == x.UserId).FirstOrDefault()?.Email,
                            address = x.Address,
                            ////lastlogin = _context.Logins.Where(x=>x.UserId == x.UserId).
                            lastorderdate = _context.Orders.OrderByDescending(f => f.Cart.UserId == x.UserId).FirstOrDefault().OrderDate,
                            customerorderlist = result?.Where(y => y.userid == x.UserId).ToList()


                        });
                    }
                    _logger.LogInformation("We Get All Customers");
                    return response;
                }
                if (Phone != null)
                {
                    var user = await _context.Users.Where(x => x.FirstName.Contains(FirstName)).ToListAsync();
                    var customer = await _context.Customers.ToListAsync();
                    var cart = await _context.Carts.ToListAsync();
                    var order = await _context.Orders.ToListAsync();
                    var login = await _context.Logins.ToListAsync();
                    var result = from us in user
                                 join c in cart on us.UserId equals c.UserId
                                 join o in order on c.CartId equals o.CartId
                                 where c.CartId == o.CartId
                                 select new CustomerOrderDTO
                                 {
                                     userid = us.UserId,
                                     ordernumber = o.OrderId.ToString(),
                                     orderdate = o.OrderDate?.ToString(),
                                     delivarydate = o.DeliveryDate?.ToString(),
                                     totalprice = o.TotalPrice?.ToString(),
                                     orderstatus =/* o.OrderStatusId?.ToString() */_context.OrderStatuses.Where(x => x.OrderStatusId == o.OrderStatusId).First().Status,

                                 };

                    List<CustomersResponseDTO?> response = new List<CustomersResponseDTO?>();
                    foreach (var x in user)
                    {
                        response.Add(new CustomersResponseDTO
                        {
                            customernamefirst = x.FirstName,
                            customernamelast = x.LastName,
                            username = _context.Customers.Where(x => x.UserId == x.UserId).FirstOrDefault()?.UserName,
                            phone = x.Phone,
                            eamil = _context.Logins.Where(y => y.UserId == x.UserId).FirstOrDefault()?.Email,
                            address = x.Address,
                            ////lastlogin = _context.Logins.Where(x=>x.UserId == x.UserId).
                            lastorderdate = _context.Orders.OrderByDescending(f => f.Cart.UserId == x.UserId).FirstOrDefault().OrderDate,
                            customerorderlist = result?.Where(y => y.userid == x.UserId).ToList()


                        });
                    }
                    _logger.LogInformation("We Get All Customers");
                    return response;
                }
                if (lastname != null)
                {
                    var user = await _context.Users.Where(x => x.LastName.Contains(lastname)).ToListAsync();
                    var customer = await _context.Customers.ToListAsync();
                    var cart = await _context.Carts.ToListAsync();
                    var order = await _context.Orders.ToListAsync();
                    var login = await _context.Logins.ToListAsync();
                    var result = from us in user
                                 join c in cart on us.UserId equals c.UserId
                                 join o in order on c.CartId equals o.CartId
                                 where c.CartId == o.CartId
                                 select new CustomerOrderDTO
                                 {
                                     userid = us.UserId,
                                     ordernumber = o.OrderId.ToString(),
                                     orderdate = o.OrderDate?.ToString(),
                                     delivarydate = o.DeliveryDate?.ToString(),
                                     totalprice = o.TotalPrice?.ToString(),
                                     orderstatus = _context.OrderStatuses.Where(x => x.OrderStatusId == o.OrderStatusId).First().Status,

                                 };

                    List<CustomersResponseDTO?> response = new List<CustomersResponseDTO?>();
                    foreach (var x in user)
                    {
                        response.Add(new CustomersResponseDTO
                        {
                            customernamefirst = x.FirstName,
                            customernamelast = x.LastName,
                            username = _context.Customers.Where(x => x.UserId == x.UserId).FirstOrDefault()?.UserName,
                            phone = x.Phone,
                            eamil = _context.Logins.Where(y => y.UserId == x.UserId).FirstOrDefault()?.Email,
                            address = x.Address,
                            ////lastlogin = _context.Logins.Where(x=>x.UserId == x.UserId).
                            lastorderdate = _context.Orders.OrderByDescending(f => f.Cart.UserId == x.UserId).FirstOrDefault().OrderDate,
                            customerorderlist = result?.Where(y => y.userid == x.UserId).ToList()


                        });
                    }
                    _logger.LogInformation("We Get All Customers");
                    return response;
                }

                else
                {
                    _logger.LogWarning("no results");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Exception");
                return new List<CustomersResponseDTO>();
            }

        }

        //public async Task<string> BannedAction(BannedDTO bannedDTO)
        //{
        //    try
        //    {
        //        CutomerBanned banned = new()
        //        {
        //            LoginId = bannedDTO.loginid,
        //            BannedAction = bannedDTO.action,
        //        };
        //        await _context.AddAsync(banned);
        //        _logger.LogInformation("Customer is Blocked");
        //        return "Banned Customer";

        //    }
        //    catch (Exception ex) 
        //    {
        //        _logger.LogError(ex.Message);
        //        return ex.Message;
        //    }
        //}


    }
}
