using Interface.DTO;
using Interface.IUnitOfWork;
using Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagement : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderManagement(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        
        }
        [AllowAnonymous]
        [HttpPost("AddToCart{userid}")]
        public async Task<IActionResult> AddToCart(int userid, int? itemid,  int? mealid, int qnty)
        {
            var cartAdd = await _unitOfWork.Ordermanagement.CreateCart(userid, itemid, mealid, qnty);
            await _unitOfWork.CompleteAsync();
            return Ok(cartAdd);
        }

        [HttpPut("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(int cartitml)
        {
            var removefromcart = await _unitOfWork.Ordermanagement.DeleteFromCart(cartitml);
            await _unitOfWork.CompleteAsync();
            return Ok(removefromcart);
        }

        [HttpPost("CheckoutOrder")]
        public async Task<IActionResult> CheckoutOrder(OrderDTO orderDTO)
        {
            var checkoutorder = await _unitOfWork.Ordermanagement.AddNewOrder(orderDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(checkoutorder);
        }

        [HttpGet("ViewOrderDetails")]
        public async Task<IActionResult> ViewOrderDetails(int PageSize, int PageNumber)
        {
            var orderGet = await _unitOfWork.Ordermanagement.Allorders();
            await _unitOfWork.CompleteAsync();
            int PageSkip = PageNumber * PageSize - PageSize;
            return Ok(orderGet.Skip(PageSkip).Take(PageSize));
        }
        #region OrderStatus
        [Authorize(Roles = "Admin")]
        [HttpPost("AddOrderStatus")]
        public async Task<IActionResult> AddOrderStatus([FromBody] string OrderStatus)
        {
            var addorderstatus = await _unitOfWork.Ordermanagement.CreateOrderStatus(OrderStatus);
            await _unitOfWork.CompleteAsync();
            return Ok(addorderstatus);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateOrderStatus{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody]string status)
        {
            var updatedstatus = await _unitOfWork.Ordermanagement.EditeOrderStatus(id, status);
            await _unitOfWork.CompleteAsync();
            return Ok(updatedstatus);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrderStatus")]
        public async Task<IActionResult> GetAllOrderStatus()
        {
            var allorderstst = await _unitOfWork.Ordermanagement.AllOrderStatus();

            List<object> response = new();
            allorderstst.ForEach(x => response.Add(new { OrderStatusId = x.OrderStatusId, Status = x.Status })); ;
            return Ok(response);
        }
        #endregion

        #region Payment

        [HttpPost("AddPaymentMethod")]
        public async Task<IActionResult> AddPaymentMethod([FromBody]string paymentway)
        {
            var addpayment = await _unitOfWork.Ordermanagement.CreatePaymentMethod(paymentway);
            await _unitOfWork.CompleteAsync();
            return Ok(addpayment);
        }
        [HttpPut("UpdatePaymentMethod{id}")]
        public async Task<IActionResult> UpdatePaymentMethod(int id, [FromBody] string paymentway)
        {
            var updatepaymeth = await _unitOfWork.Ordermanagement.EditePaymentMethod(id, paymentway);
            await _unitOfWork.CompleteAsync();
            return Ok(updatepaymeth);
        }
        [HttpGet("GetPaymentMethod")]
        public async Task<IActionResult> GetPaymentMethod()
        {
            var getpaymeth = await _unitOfWork.Ordermanagement.AllPayment();
            List<object> response = new();
            getpaymeth.ForEach(x => response.Add(new
            {
                PaymentMethodID = x.PaymentId,
                PAymentMethod = x.PaymentMethod
            }));
            return Ok(response);
        }
        #endregion
    }
}
