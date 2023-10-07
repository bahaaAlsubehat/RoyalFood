using Interface.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.IRepository
{
    
    public interface ISalesDashboard
    {
        #region SalesDashboard
        Task<Dictionary<string, float>> DailySalesChart();
        Task<Dictionary<string, float>> MonthlySalesChart(DateTime StartMonth, DateTime LastMonth);
        Task<Dictionary<string, int>> DailyDeliveredOrders(DateTime today1);
        Task<Dictionary<string, int>> MonthlyDeliveredOrders(DateTime stratDate, DateTime LastDate);
        Task<string> ActivatedOrders();
        #endregion

        Task<List<object>> TopItems();
        Task<List<object>> TopMeals();
        Task<List<object>> TopCategoryItem();
        Task<List<object>> TopCategoriesMeals();
    }
}
