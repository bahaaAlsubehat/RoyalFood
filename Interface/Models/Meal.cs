using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Meal
    {
        public Meal()
        {
            CartItemMeals = new HashSet<CartItemMeal>();
            ImageMeals = new HashSet<ImageMeal>();
            ItemMeals = new HashSet<ItemMeal>();
        }

        public int MealId { get; set; }
        public string? MealName { get; set; }
        public string? MealNameAr { get; set; }
        public string? MealDescription { get; set; }
        public string? MealDescriptionAr { get; set; }
        public int? CategoryId { get; set; }
        public bool? Availability { get; set; }
        public double? Price { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public int? LastModifiedUserId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual User? LastModifiedUser { get; set; }
        public virtual ICollection<CartItemMeal> CartItemMeals { get; set; }
        public virtual ICollection<ImageMeal> ImageMeals { get; set; }
        public virtual ICollection<ItemMeal> ItemMeals { get; set; }
    }
}
