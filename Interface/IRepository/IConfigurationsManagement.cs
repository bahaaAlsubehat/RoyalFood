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

        #region Role
        Task<List<Role>> AllRoles();
        Task<string> AddRole(RoleDTO roleDTO);
        Task<string> EditeRole(int id, RoleDTO roleDTO);
        Task<List<Role>> SortRole(int? id, string? Name, string? permission);

        #endregion

        #region UserAndAuthantication
        Task<List<User>> AllUsers();
        Task<string> AddUser(RegisterDTO registerDTO);
        Task<string> Signin(LoginDTO loginDTO);
        Task<string> UpdatePassword(ResetPasswordDTO resetPasswordDTO);
        Task<string> PutNewPaaword(ForgetPasswordDTO forgetPasswordDTO);
        Task<List<User>> SortUsers(int? roleid, string? fname, string? lastname, string? address);
        Task<string> Logount(LoginDTO loginDTO);
        #endregion

        Task<string> CreateIngredient(IngredientDTO ingredientDTO);
        Task<string> EditeIngredient(int id, IngredientDTO ingredientDTO);
        Task<List<Ingredient>> ViewIngredients();
        Task<List<Ingredient>> SortIngredient(int? id, string? ingName);
    }
}
