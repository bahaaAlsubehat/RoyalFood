using Interface.DTO;
using Interface.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.IRepository
{
    public interface IConfigurationsManagement
    {
        Task<List<Role>> AllRoles();
        Task<string> AddRole(RoleDTO roleDTO);
        Task<string> EditeRole(int id, RoleDTO roleDTO);
        Task<List<Role>> SortRole(int? id, string? Name, string? permission);
    }
}
