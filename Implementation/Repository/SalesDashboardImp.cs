using Interface.DTO;
using Interface.IRepository;
using Interface.IUnitOfWork;
using Interface.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Repository
{
    public class SalesDashboardImp: ISalesDashboard
    {
        private readonly RoyalFoodContext _context;
        private readonly ILogger<SalesDashboardImp> _logger;
        public SalesDashboardImp(RoyalFoodContext context, ILogger<SalesDashboardImp> logger) 
        {
            _context = context;
            _logger = logger;
        }

        #region SalesDashboard
        public async Task<Dictionary<string, float>> DailySalesChart()
        {
            try
            {
                // Retrieve order data from the database
                List<Order> orders = _context.Orders.ToList();

                // Filter orders for the past 30 days based on delivery date
                DateTime endDate = DateTime.UtcNow.Date;
                DateTime startDate = endDate.AddDays(-30);
                List<Order> filteredOrders = orders.Where(o => o.DeliveryDate >= startDate && o.DeliveryDate <= endDate).ToList();

                // Calculate total sales for each day
                Dictionary<string, float> dailySales = new Dictionary<string, float>();
                foreach (Order order in filteredOrders)
                {
                    string dayMonth = order.DeliveryDate.ToString();
                    if (dailySales.ContainsKey(dayMonth))
                    {
                        dailySales[dayMonth] += (float)order.TotalPrice;
                    }
                    else
                    {
                        dailySales[dayMonth] = (float)order.TotalPrice;
                    }
                }



                return dailySales;

                //{
                //    var orders = await _context.Orders.ToListAsync();
                //    DateTime thirtyDaysAgo = DateTime.Today.AddDays(-30);
                //    var filteredOrders = orders.Where(o => o.DeliveryDate >= thirtyDaysAgo);

                //    // Calculate daily sales
                //    var dailySales = filteredOrders
                //        .GroupBy(o => o.DeliveryDate)  // Group by date (ignoring time)
                //        .Select(g => new
                //        {
                //            Date = g.Key,
                //            TotalSales = g.Sum(o => o.TotalPrice)
                //        })
                //        .OrderBy(g => g.Date).ToList();

                //    return dailySales<DateTime,TotalSales>;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Exception{ex.Message}");
                return null;
            }

        }

        public async Task<Dictionary<string, float>> MonthlySalesChart(DateTime StartMonth, DateTime LastMonth)
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                //List<Order> orders = new List<Order>();
                List<Order> filteredOrders = orders.Where(o => o.DeliveryDate >= StartMonth && o.DeliveryDate <= LastMonth).ToList();
                Dictionary<string, float> monthlySales = new Dictionary<string, float>();
                foreach (Order order in filteredOrders)
                {
                    string Month = StartMonth.ToString() + " to " + LastMonth.ToString();
                    if (monthlySales.ContainsKey(Month))
                    {
                        monthlySales[Month] += (float)order.TotalPrice;
                    }
                    else
                    {
                        monthlySales[Month] = (float)order.TotalPrice;

                    }
                }



                return monthlySales;



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Dictionary<string, int>> DailyDeliveredOrders(DateTime today1)
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                var countdelivered = orders.Where(x => x.DeliveryDate <= today1 && x.OrderStatusId == 4).Count();
                Dictionary<string, int> orderDeliveredCounts = new Dictionary<string, int>();
                string DateCount = today1.ToString();
                orderDeliveredCounts[DateCount] = countdelivered;
                _logger.LogInformation("All Daily Delivered Orders");
                return orderDeliveredCounts;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, int>();
            }
        }

        // error below function :MonthlyDeliveredOrders
        public async Task<Dictionary<string, int>> MonthlyDeliveredOrders(DateTime stratDate, DateTime LastDate)
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                var monthlydelivered = orders.Where(x => (x.DeliveryDate >= stratDate && x.DeliveryDate <= LastDate) && x.OrderStatusId == 4).ToList();
                Dictionary<string, int> orderDeliveredCounts = new Dictionary<string, int>();
                string DateCount = stratDate.ToString() + "_" + LastDate.ToString();
                //orderDeliveredCounts[DateCount] = monthlydelivered;
                foreach (Order order in monthlydelivered)
                {
                    if (orderDeliveredCounts.ContainsKey(DateCount))
                    {
                        orderDeliveredCounts[DateCount] += monthlydelivered.Count();
                    }
                    else
                    {
                        orderDeliveredCounts[DateCount] = monthlydelivered.Count();
                    }


                }



                _logger.LogInformation("All Mothly Delivered Orders");
                return orderDeliveredCounts;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("error");
                return new Dictionary<string, int>();
            }
        }

        public async Task<string> ActivatedOrders()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                float NewPercentege = 0;
                float ConfirmPercentage = 0;
                float countN = 0;
                float countC = 0;
                //Dictionary<int, float> GetNewConfirmOrders = new Dictionary<int, float>();
                foreach (var order in orders)
                {
                    //int ordersts = (int)order.OrderStatusId;
                    if (order.OrderStatusId == 1)
                    {
                        countN = orders.Count();
                        NewPercentege = countN / 100;
                    }
                    if (order.OrderStatusId == 2)
                    {
                        countC = orders.Count();
                        ConfirmPercentage = countC / 100;
                    }
                    return $"All New Orders count is {countN} and Percentage equal {NewPercentege}, " + $"All New Orders count is {countC} and Percentage equal {ConfirmPercentage}";


                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("error");
                return ex.Message;
            }
        }
        #endregion


        public async Task<List<object>> TopItems()
        {
            try
            {
                var carts = await _context.Carts.Where(x => x.IsActive == false).ToListAsync();
                var cartitems = await _context.CartItemMeals.ToListAsync();
                var items = await _context.Items.ToListAsync();
                var top10 = (from c in carts
                             join ci in cartitems on c.CartId equals ci.CartId
                            join i in items on ci.ItemId equals i.ItemId
                            select new TopItemDTO
                            {
                                itemid = i.ItemId,
                                itemname = i.ItemName,
                                qny = cartitems.Where(u => u.ItemId == i.ItemId).Sum(c => c.Quantity)
                            }).OrderByDescending(m=>m.qny).DistinctBy(n=>n.itemid).Take(10);
                List<object> response = new List<object>();
                foreach (var itop in top10)
                {
                    if(carts.Any(x=>x.IsActive == false))
                    {
                        response.Add(itop);

                    }
                }
                return response;

            }
            catch (Exception ex)
            {

                _logger.LogWarning($"error {ex.Message}");
                return null;
            }
        }
        public async Task<List<object>> TopMeals()
        {
            try
            {
                var carts = await _context.Carts.Where(x => x.IsActive == false).ToListAsync();
                var cartmeals = await _context.CartItemMeals.ToListAsync();
                var meals = await _context.Meals.ToListAsync();
                var TopMeals = (from c in carts
                                join cm in cartmeals on c.CartId equals cm.CartId
                                join m in meals on cm.MealId equals m.MealId
                                select new TopMealsDTO
                                {
                                    mealid = m.MealId,
                                    mealname = m.MealName,
                                    qnty = cartmeals.Where(x => x.MealId == m.MealId).Sum(c => c.Quantity)
                                }).OrderByDescending(t => t.qnty).DistinctBy(r => r.mealid).Take(10);
                List<object> response = new List<object>();
                foreach (var tm in TopMeals)
                {
                    response.Add(tm);
                }
                _logger.LogInformation("We Get Top 10 Meals Sales");
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new List<object>();

            }
            


        }

        public async Task<List<object>> TopCategoryItem()
        {
            try
            {
                var carts = await _context.Carts.Where(x=>x.IsActive == false).ToListAsync();
                var cartitems = await _context.CartItemMeals.ToListAsync();
                var items = await _context.Items.ToListAsync();
                var categories = await _context.Categories.ToListAsync();
                var results = (from c in carts
                               join ci in cartitems on c.CartId equals ci.CartId
                               join i in items on ci.ItemId equals i.ItemId
                               join ctg in categories on i.CategoryId equals ctg.CategoryId
                               where i.CategoryId == ctg.CategoryId
                               select new TopCategoryItemDTO
                               {
                                   itemid = i.ItemId,
                                   categoryid = items.Where(x => x.CategoryId == ctg.CategoryId).First().CategoryId, /*categories.Where(x=>x.CategoryId == i.CategoryId).First().CategoryId,*/
                                   categoryname = ctg.CategoryName, /*categories.Where(x => x.CategoryId == ctg.CategoryId).First().CategoryName,*/
                                   qnty = cartitems.Where(u => u.ItemId == i.ItemId).Sum(l => l.Quantity),
                                  
                               }).OrderByDescending(v => v.qnty).DistinctBy(c => c.itemid).GroupBy(f=>f.categoryid).Take(10);

                List<object> result = new List<object>();
                foreach (var item in results) 
                {
                    result.Add(item);
                    result.Add(item.Sum(x => x.qnty));
                }
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<List<object>> TopCategoriesMeals()
        {
            try
            {
                var carts = await _context.Carts.Where(x => x.IsActive == false).ToListAsync();
                var cartitems = await _context.CartItemMeals.ToListAsync();
                var meals = await _context.Meals.ToListAsync();
                var categories = await _context.Categories.ToListAsync();
                var top10 = (from c in carts
                             join ci in cartitems on c.CartId equals ci.CartId
                             join m in meals on ci.MealId equals m.MealId
                             join ctg in categories on m.CategoryId equals ctg.CategoryId
                             where m.CategoryId == ctg.CategoryId
                             select new TopCategoriesMealDTO
                             {
                                 mealid = m.MealId,
                                 categoryid = meals.Where(c=>c.CategoryId == ctg.CategoryId).First().CategoryId,
                                 categoryname = ctg.CategoryName,
                                 qnty = cartitems.Where(u => u.MealId == m.MealId).Sum(l => l.Quantity),


                             }).OrderByDescending(v => v.qnty).DistinctBy(c => c.mealid).GroupBy(f => f.categoryid).Take(10);

                List<object> result = new List<object>();
                foreach (var item in top10)
                {
                    result.Add(item);
                    result.Add(item.Sum(x => x.qnty));
                }
                _logger.LogInformation("to 10 categories sales");
                return result;
            }
            catch(Exception ex) 
            {
                _logger.LogError($"{ex.Message}");
                return null;

            }
        }


    }
}
