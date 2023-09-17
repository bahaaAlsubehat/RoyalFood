using Interface.DTO;
using Interface.IRepository;
using Interface.Models;
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
    public class ConfigurationsManagementImp : IConfigurationsManagement
    {
        private readonly RoyalFoodContext _context;
        private readonly ILogger<ConfigurationsManagementImp> _logger;
            
        public ConfigurationsManagementImp(RoyalFoodContext context,ILogger<ConfigurationsManagementImp> logger) 
        { 
            _context = context;
            _logger = logger;
        
        }
        public async Task<List<Role>> AllRoles()
        {
            try
            {
                var RolesAll = await _context.Roles.ToListAsync();
                return RolesAll;
                _logger.LogInformation("We Get All Roles");
            }
            catch (Exception ex)
            { 
                return null;
                _logger.LogError(ex.Message);
            
            }

        }
        public async Task<string> AddRole(RoleDTO roleDTO)
        {
            try
            {
                if(await _context.Roles.AnyAsync(x=>x.RoleName == roleDTO.name))
                {
                    return $"Name Role is Already Exsist{roleDTO.name}";


                }
                else
                {
                    Role role = new() 
                    {
                        RoleName = roleDTO.name,
                        Permissions = roleDTO.permission
                    
                    };
                    await _context.AddAsync(role);
                    return "Created Role";
                }
                _logger.LogInformation("Done");
            }
            catch (Exception ex) 
            {
                return null;
                _logger.LogError($"{ex.Message}");
            
            }
        }

        public async Task<string> EditeRole(int id, RoleDTO roleDTO)
        {
            try
            {
                var editeRole = await _context.Roles.SingleOrDefaultAsync(x => x.RoleId == id);
                if (editeRole != null)
                {
                    editeRole.RoleName = roleDTO.name;
                    editeRole.Permissions =roleDTO.permission;
                    _context.Update(editeRole);
                    _logger.LogInformation($"Updated on Role id: {id}");
                    return "Updated Successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
                
            }
        }
        public async Task<List<Role>> SortRole(int? id, string? Name, string? permission)
        {
            var SortingRole = await _context.Roles.ToListAsync();
            try 
            {
                if (SortingRole == null)
                {
                    return null;
                }
                else
                {
                    if (id != null)
                    {
                        SortingRole = await _context.Roles.Where(x => x.RoleId == id).ToListAsync();
                    }
                    if (Name != null)
                    {
                        SortingRole = await _context.Roles.Where(x => x.RoleName.Contains(Name)).ToListAsync();

                    }
                    if (permission != null)
                    {
                        SortingRole = await _context.Roles.Where(x => x.Permissions.Contains(permission)).ToListAsync();

                    }
                    return SortingRole;
                }
                _logger.LogInformation("Filterring Roles");
            }
            catch (Exception ex)
            { 
                return null;  
                _logger.LogError(ex.Message);
            } 
            
           
        }


    }
}
