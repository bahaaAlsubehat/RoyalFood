using Interface.DTO;
using Interface.IUnitOfWork;
using Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RoyalFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsManagement : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConfigurationsManagement(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        #region Roles

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var GetRoles = await _unitOfWork.Configurationsmanagement.AllRoles();
            await _unitOfWork.CompleteAsync();
            return Ok(GetRoles);
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleDTO roleDTO)
        {
            var RoleCreate = await _unitOfWork.Configurationsmanagement.AddRole(roleDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(RoleCreate);
        }

        [HttpPut]
        [Route("UpdateRole")]
        public async Task<IActionResult> UpdateRole(int id, RoleDTO roleDTO)
        {
            var updateRole = await _unitOfWork.Configurationsmanagement.EditeRole(id, roleDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(updateRole);
        }

        [HttpGet]
        [Route("Filter")]
        public async Task<IActionResult> FilterRole(int? id, string? Name, string? permission)
        {
            var FilterringRoles = await _unitOfWork.Configurationsmanagement.SortRole(id, Name, permission);
            await _unitOfWork.CompleteAsync();
            return Ok(FilterringRoles);
        }

        #endregion

        #region UserAndAuthantication
        [HttpGet]
        [Route("GetUsers")]

        public async Task<IActionResult> GetUsers(int PageSize, int PageNumber)
        {
            List<object> Response = new();


            (await _unitOfWork.Configurationsmanagement.AllUsers()).ForEach(x => Response.Add(new {UserId =x.UserId, FirstName = x.FirstName, LastName = x.LastName, Phone = x.Phone, Address = x.Address, Gender = x.Gender, Age = x.Age, Roleid = x.RoleId , ImageProfile = x.ProfileImage}));

            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(Response.Skip(PageSkip).Take(PageSize));
        }

        [HttpGet]
        [Route("GetUser")]

        public async Task<IActionResult> GetUser([FromQuery] int id)
        {
            var userr = (await _unitOfWork.Configurationsmanagement.AllUsers()).Where(x => x.UserId == id).FirstOrDefault();
            await _unitOfWork.CompleteAsync();
            return Ok(new { FirstName = userr.FirstName, LastName = userr.LastName, Phone = userr.Phone, Age = userr.Age, Gender = userr.Gender, rolie = userr.RoleId ,ImageProfile = userr.ProfileImage });
        }




        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(RegisterDTO registerDTO)
        {
            var AddUser = await _unitOfWork.Configurationsmanagement.AddUser(registerDTO);
            await _unitOfWork.CompleteAsync();
            return Ok("Successfully Added User");

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            string token = await _unitOfWork.Configurationsmanagement.Signin(loginDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(token);
        }

        [HttpPut]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var RestPass = await _unitOfWork.Configurationsmanagement.UpdatePassword(resetPasswordDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(RestPass);
        }

        [HttpPut]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPasword(ForgetPasswordDTO forgetPasswordDTO)
        {
            var forgpass = await _unitOfWork.Configurationsmanagement.PutNewPaaword(forgetPasswordDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(forgpass);
        }

        [HttpGet]
        [Route("FilterUsers")]
        public async Task<IActionResult> FilterUsers(int? roleid, string? fname, string? lastname, string? address, int PageSize, int PageNumber)
        {
            List<object> Response = new();

            var filterUsers = await _unitOfWork.Configurationsmanagement.SortUsers(roleid, fname, lastname, address);
            filterUsers.ForEach(x => Response.Add(new { UserId = x.UserId, FirstName = x.FirstName, LastName = x.LastName, Address = x.Address, Gender = x.Gender, Age = x.Age, RoleId = x.RoleId, Phone = x.Phone ,ImageProfile= x.ProfileImage}));
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(Response.Skip(PageSkip).Take(PageSize));

        }
        [AllowAnonymous]
        [HttpPut]
        [Route("Logout")]
        public async Task<IActionResult> Logount([FromHeader] string token)
        {
            var logout1 =await  _unitOfWork.Configurationsmanagement.Logount(token);
            await _unitOfWork.CompleteAsync();
            return Ok(logout1);
        }
        #endregion

        #region Ingredient
        [HttpPost("AddIngredient")]
        public async Task<IActionResult> AddIngredient(IngredientDTO ingredientDTO)
        {
            var Addingred = await _unitOfWork.Configurationsmanagement.CreateIngredient(ingredientDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(Addingred);
        }

        [HttpPut("UpdateIngredient")]
        public async Task<IActionResult> UpdateIngredient(int id, IngredientDTO ingredientDTO)
        {
            var UpdateIngred = await _unitOfWork.Configurationsmanagement.EditeIngredient(id, ingredientDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(UpdateIngred);
        }
        [HttpGet("GetIngredientes")]
        public async Task<IActionResult> GetIngredients(int PageSize, int PageNumber)
        {
            List<object> response = new();
            (await _unitOfWork.Configurationsmanagement.ViewIngredients()).ForEach(x => response.Add(new { IngredientId = x.IngredientId, IngredientName = x.Name, IngredientNameAr=x.NameAr, IngredientDecreption = x.Describtion,
            IngredientDecreptionAr = x.DescribtionAr, Unit = x.Unit, ImageId = x.ImageId }));
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }

        [HttpGet("FilterIngredient")]
        public async Task<IActionResult> FilterIngredient(int PageSize, int PageNumber, int? id, string? ingName, string? ingNameAr)
        {
            List<object> response = new();
            (await _unitOfWork.Configurationsmanagement.SortIngredient(id, ingName, ingNameAr)).ForEach(x => response.Add(new { IngredientId = x.IngredientId, IngredientName = x.Name, IngredientNameAr =x.NameAr, IngredientDecreption = x.Describtion, IngredientDecreptionAr=x.DescribtionAr,  Unit = x.Unit, ImageId = x.ImageId }));
            await _unitOfWork.CompleteAsync();

            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }

        #endregion

        #region Category
        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            var Createcategory = await _unitOfWork.Configurationsmanagement.AddCategory(categoryDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(Createcategory);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var Updatecat = await _unitOfWork.Configurationsmanagement.EditeCategory(id, categoryDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(Updatecat);
        }
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories(int PageSize, int PageNumber)
        {
            List<object> response = new();

            (await _unitOfWork.Configurationsmanagement.AllCategories()).ForEach(x => response.Add(new { CategoryId = x.CategoryId, CategoryName = x.CategoryName, CategoryNameAr=x.CategoryNameAr, CategoryDescription = x.Description, CategoryDescriptionAr =x.DescriptionAr, IsActive = x.IsActive, ImageId = x.ImageId }));
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }
        [HttpGet("FilterCategories")]

        public async Task<IActionResult> FilterUsers(int? id, string? Name,string? NameAr, int PageSize, int PageNumber)
        {
            List<object> response = new();

            (await _unitOfWork.Configurationsmanagement.SortCategory(id, Name, NameAr)).ForEach(x => response.Add(new { CategoryId = x.CategoryId, CategoryName = x.CategoryName, CategoryNameAr = x.CategoryNameAr, CategoryDescription = x.Description, CategoryDescriptionAr=x.DescriptionAr, IsActive = x.IsActive, ImageId = x.ImageId }));
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }
        #endregion

        #region Image
        [Authorize(Roles="Admin")]
        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage(ImageDTO imageDTO)
        {
            var AddImage = await _unitOfWork.Configurationsmanagement.CreateImagePath(imageDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(AddImage);
        }
        [HttpPut("UpdateImage")]
        public async Task<IActionResult> UpdateImage(int id, ImageDTO imageDTO)
        {
            var updateimg = await _unitOfWork.Configurationsmanagement.EditeImage(id, imageDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(updateimg);
        }
        [HttpGet("GetImages")]
        public async Task<IActionResult> GetImages(int PageSize, int PageNumber)
        {
            List<object> response = new();
            (await _unitOfWork.Configurationsmanagement.AllImages()).ForEach(x => response.Add(new { ImageId = x.ImageId, ImagePath = x.Path, IsDefault = x.IsDefault }));
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }
        [HttpGet("FilterImages")]
        public async Task<IActionResult> FilterImage(int PageNumber, int PageSize, int? id, string? Path)
        {
            List<object> response = new();
            (await _unitOfWork.Configurationsmanagement.SortImages(id, Path)).ForEach(x => response.Add(new { ImageId = x.ImageId, ImagePath = x.Path, IsDefault = x.IsDefault }));
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }
        #endregion

        #region ItemImages
        [HttpPost("AddItemImage")]
        public async Task<IActionResult> AddItemImage(ItemImageDTO itemimgDTO)
        {
            var addimitem = await _unitOfWork.Configurationsmanagement.CreateItemImage(itemimgDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(addimitem);
        }


        [HttpPut("UpdateItemImage")]
        public async Task<IActionResult> UpdateItemImage(int id, ItemImageDTO itemimgDTO)
        {
            var updateimgitem = await _unitOfWork.Configurationsmanagement.EditeItemImage(id, itemimgDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(updateimgitem);
        }
        #endregion

        #region MealImages
        [HttpPost("AddMealImage")]
        public async Task<IActionResult> AddMealImage(MealImageDTOcs mealimageDTO)
        {
            var addimgmeal = await _unitOfWork.Configurationsmanagement.CreateMealImage(mealimageDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(addimgmeal);
        }

        [HttpPut("UpdateMealImage")]
        public async Task<IActionResult> UpdateMealImage(int id, MealImageDTOcs mealimageDTO)
        {
            var updateimgmeal = await _unitOfWork.Configurationsmanagement.EditeMealImage(id, mealimageDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(updateimgmeal);
        }
        #endregion




    }
}
