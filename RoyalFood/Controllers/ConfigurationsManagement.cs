using Interface.DTO;
using Interface.IUnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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



    }
}
