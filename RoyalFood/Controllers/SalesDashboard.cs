using Implementation.UnitOfWork;
using Interface.IUnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesDashboard : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;

        public SalesDashboard(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;

        }

        #region SalesDashboard
        [HttpGet("DailySales")]
        public async Task<IActionResult> DailySales()
        {
            var dailsale = await _unitOfWork.Salesdashboard.DailySalesChart();
            await _unitOfWork.CompleteAsync();
            return Ok(dailsale);
        }

        [HttpGet("MonthlySales")]
        public async Task<IActionResult> MonthlySales(DateTime StartMonth, DateTime LastMonth)
        {
            var Monthsale = await _unitOfWork.Salesdashboard.MonthlySalesChart(StartMonth, LastMonth);
            await _unitOfWork.CompleteAsync();
            return Ok(Monthsale);
        }

        [HttpGet("DailyOrderDelivered")]
        public async Task<IActionResult> DailyOrderDelivered(DateTime today)
        {
            var DOD = await _unitOfWork.Salesdashboard.DailyDeliveredOrders(today);
            await _unitOfWork.CompleteAsync();
            return Ok(DOD);
        }

        [HttpGet("MonthlyOrdersDelivered")]
        public async Task<IActionResult> MonthlyOrdersDelivered(DateTime stratDate, DateTime LastDate)
        {
            var MOD = await _unitOfWork.Salesdashboard.MonthlyDeliveredOrders(stratDate, LastDate);
            await _unitOfWork.CompleteAsync();
            return Ok(MOD);
        }

        [HttpGet("ActiveOrders")]
        public async Task<IActionResult> ActiveOrders()
        {
            var NCO = await _unitOfWork.Salesdashboard.ActivatedOrders();
            await _unitOfWork.CompleteAsync();
            return Ok(NCO);
        }
        #endregion

        [HttpGet("ItemsTop")]
        public async Task<IActionResult> ItemsTop()
        {
            var top10Items = await _unitOfWork.Salesdashboard.TopItems();
            await _unitOfWork.CompleteAsync();
            return Ok(top10Items);
        }

        [HttpGet("MealsTop")]
        public async Task<IActionResult> MealsTop()
        {
            var Top10Meals =  await _unitOfWork.Salesdashboard.TopMeals();
            await _unitOfWork.CompleteAsync();
            return Ok(Top10Meals);
        }

        [HttpGet("CategoryItemTop10")]
        public async Task<IActionResult> CategoryItemTop10()
        {
            var topcategory10 = await _unitOfWork.Salesdashboard.TopCategoryItem();
            await _unitOfWork.CompleteAsync();
            return Ok(topcategory10);
        }

        [HttpGet("CategoryMealsTop10")]
        public async Task<IActionResult> CategoryMealsTop10()
        {
            var cateorymealtop10 = await _unitOfWork.Salesdashboard.TopCategoriesMeals();
            await _unitOfWork.CompleteAsync();
            return Ok(cateorymealtop10);
        }

        [HttpGet("Top10Items7Days")]
        public async Task<IActionResult> Top10Items7Days()
        {
            var Resultin7Days = await _unitOfWork.Salesdashboard.ItemsTop10in7Days();
            await _unitOfWork.CompleteAsync();
            return Ok(Resultin7Days);
        }
    }
}
