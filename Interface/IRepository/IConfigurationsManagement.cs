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
        Task<bool> Logount(string token);
         
        #endregion

        #region Ingredient
        Task<string> CreateIngredient(IngredientDTO ingredientDTO);
        Task<string> EditeIngredient(int id, IngredientDTO ingredientDTO);
        Task<List<Ingredient>> ViewIngredients();
        Task<List<Ingredient>> SortIngredient(int? id, string? ingName,string? ingNameAr);
        #endregion

        #region Category
        Task<string> AddCategory(CategoryDTO categoryDTO);
        Task<string> EditeCategory(int id, CategoryDTO categoryDTO);
        Task<List<Category>> AllCategories();
        Task<List<Category>> SortCategory(int? id, string? Name, string?NameAr);
        #endregion

        #region Image
        Task<string> CreateImagePath(ImageDTO imageDTO);
        Task<string> EditeImage(int id, ImageDTO imageDTO);
        Task<List<Image>> AllImages();
        Task<List<Image>> SortImages(int? id, string? Path);
        #endregion

        #region ItemImages
        Task<string> CreateItemImage(ItemImageDTO itemimgDTO);
        Task<string> EditeItemImage(int id, ItemImageDTO itemimgDTO);
        #endregion

        #region MealImages
        Task<string> CreateMealImage(MealImageDTOcs mealimageDTOt);
        Task<string> EditeMealImage(int id, MealImageDTOcs mealimageDTO);
        #endregion



    }
}
