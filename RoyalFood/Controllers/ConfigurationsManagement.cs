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


            (await _unitOfWork.Configurationsmanagement.AllUsers()).ForEach(x => Response.Add(new { FirstName = x.FirstName, LastName = x.LastName, Phone = x.Phone, Address = x.Address, Gender = x.Gender, Age = x.Age, Roleid = x.RoleId }));

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
            return Ok(new { FirstName = userr.FirstName, LastName = userr.LastName, Phone = userr.Phone, Age = userr.Age, Gender = userr.Gender, rolie = userr.RoleId });
        }




        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(RegisterDTO registerDTO)
        {
            var AddUser = await _unitOfWork.Configurationsmanagement.AddUser(registerDTO);
            await _unitOfWork.CompleteAsync();
            return Ok("Successfully Added User");

        }
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
            filterUsers.ForEach(x => Response.Add(new { FirstName = x.FirstName, LastName = x.LastName, Address = x.Address, Gender = x.Gender, Age = x.Age, RoleId = x.RoleId, Phone = x.Phone }));
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(Response.Skip(PageSkip).Take(PageSize));

        }
        [HttpPut]
        [Route("Logout")]
        public async Task<IActionResult> Logount(LoginDTO loginDTO)
        {
            return Ok();
        }
        #endregion

        [HttpPost("AddIngredient")]
        public async Task<IActionResult> AddIngredient(IngredientDTO ingredientDTO)
        {
            var Addingred = await _unitOfWork.Configurationsmanagement.CreateIngredient(ingredientDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(Addingred);
        }

        [HttpPut("UpdateIngredient")]
        public async Task<IActionResult> UpdateIngredient(int id,IngredientDTO ingredientDTO)
        {
            var UpdateIngred = await _unitOfWork.Configurationsmanagement.EditeIngredient(id, ingredientDTO);
            await _unitOfWork.CompleteAsync();
            return Ok(UpdateIngred);
        }
        [HttpGet("GetIngredientes")]
        public async Task<IActionResult> GetIngredients(int PageSize, int PageNumber)
        {
            List<object> response = new();
            (await _unitOfWork.Configurationsmanagement.ViewIngredients()).ForEach(x=>response.Add(new {IngredientId =x.IngredientId, IngredientName =x.Name, IngredientDecreption=x.Describtion, Unit=x.Unit, ImageId=x.ImageId}));
            await _unitOfWork.CompleteAsync();
            int PageSkip = (PageNumber*PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }

        [HttpGet("FilterIngredient")]
        public async Task<IActionResult> FilterIngredient(int PageSize, int PageNumber, int? id, string? ingName)
        {
            List<object> response = new();
            (await _unitOfWork.Configurationsmanagement.SortIngredient(id, ingName)).ForEach(x=>response.Add(new { IngredientId = x.IngredientId, IngredientName = x.Name, IngredientDecreption = x.Describtion, Unit = x.Unit, ImageId = x.ImageId }));
            await _unitOfWork.CompleteAsync();

            int PageSkip = (PageNumber * PageSize) - PageSize;
            return Ok(response.Skip(PageSkip).Take(PageSize));
        }
    }
}
