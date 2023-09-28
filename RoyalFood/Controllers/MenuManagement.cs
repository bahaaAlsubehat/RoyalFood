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
    public class MenuManagement : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public MenuManagement(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region Item
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem(ItemDTO itemDTO)
        {
            var Additem = await _unitOfWork.Menumanagement.CreateItem(itemDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(Additem);
        }
        [AllowAnonymous]
        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateItem(int id, ItemDTO itemDTO)
        {
            var updatedItem = await _unitOfWork.Menumanagement.EditeItem(id, itemDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(updatedItem);
        }
        [HttpGet("GetItems")]
        public async Task<IActionResult> GetItems(int PageSize, int PageNumber)
        {
            var Getitems = await _unitOfWork.Menumanagement.AllItems();
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber*PageSize)-PageSize;
            return Ok(Getitems.Skip(PageSkip).Take(PageSize));
        }
        [HttpGet("FilterItems")]
        public async Task<IActionResult> FilterItems(int PageSize, int PageNumber, int? id, string? itemname, string? itemnamear, string? description, string? descriptionAr, float? price)
        {
            var filterItems = await _unitOfWork.Menumanagement.SortItems(id, itemname,itemnamear, description,descriptionAr, price);
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber*PageSize)-PageSize;
            return Ok(filterItems.Skip(PageSkip).Take(PageSize));
        }

        #endregion

        #region Meal
        [Authorize(Roles = "Admin")]
        [HttpPost("AddMeal")]
        public async Task<IActionResult> AddMeal(MealDTO maelDTO)
        {
            var addedMeal = await _unitOfWork.Menumanagement.CreateMeal(maelDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(addedMeal);
        }
        [HttpPut("UpdateMeal")]
        public async Task<IActionResult> UpdateMeal(int id, MealDTO mealDTO)
        {
            var updatedmeal = await _unitOfWork.Menumanagement.EditeMeal(id, mealDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(updatedmeal);
        }
        [HttpGet("GetAllMeal")]
        public async Task<IActionResult> GetAllMeal(int PAgeSize, int PageNumber)
        {
            var getallMeals = await _unitOfWork.Menumanagement.AllMeals();
            int PageSkip = (PAgeSize * PageNumber) - PAgeSize;
            await _unitOfWork.CompleteAsync();
            return Ok(getallMeals.Take(PageSkip).Skip(PAgeSize));
        }
        [HttpGet("FilterMeals")]
        public async Task<IActionResult> FilterMeals(int PAgeSize, int PageNumber, int? id, string? namear, string? nameing , string? descar, string? descing, float? price)
        {
            return Ok();
        }
        #endregion


    }
}
