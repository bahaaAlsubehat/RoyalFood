using Interface.DTO;
using Interface.IUnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerManagement : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerManagement(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        
        }

        [HttpGet("ViewAllCustomers")]
        public async Task<IActionResult> ViewAllCustomers(int PageSize, int PageNumber)
        {
            var getCustomers = await _unitOfWork.Customersmanagement.AllCustomers();
            await _unitOfWork.CompleteAsync();
            int PageSkip = PageNumber * PageSize - PageSize;
            return Ok(getCustomers.Skip(PageSkip).Take(PageSize));
        }

        [HttpGet("FilterCustomers")]
        public async Task<IActionResult> FilterCustomers(int PageSize, int PageNumber,int? CustomerId, string? FirstName, string? Phone, string? lastname)
        {
            var filtercustomer = await _unitOfWork.Customersmanagement.SortCustomer(CustomerId, FirstName, Phone, lastname);
            await _unitOfWork.CompleteAsync();
            return Ok(filtercustomer);
        }

        //[HttpPost("BannedCustomer")]
        //public async Task<IActionResult> BannedCustomer(BannedDTO bannedDTO)
        //{
        //    var banned = await _unitOfWork.Customersmanagement.BannedAction(bannedDTO);
        //    await _unitOfWork.CompleteAsync();
        //    return Ok(banned);
        //}



    }
}
