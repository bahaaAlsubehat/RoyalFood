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
    public interface IMenuManagement
    {
        #region Item
        Task<string> CreateItem(ItemDTO itemDTO);
        Task<string> EditeItem(int id, ItemDTO itemDTO);
        Task<List<ItemGetDTO>> AllItems();
        Task<List<ItemGetDTO>> SortItems(int? id, string? itemname, string? itemnamear, string? description, string? descriptionAr, float? price);
        #endregion

        #region Meal
        Task<string> CreateMeal(MealDTO maelDTO);
        Task<string> EditeMeal(int id, MealDTO mealDTO);
        Task<List<MealGet>> AllMeals();
        Task<List<MealGet>> SortungMeal(int? id, string? namear, string? nameing, string? descar, string? descing, float? price);
        #endregion


    }
}
