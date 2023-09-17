using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Meal
    {
        public Meal()
        {
            CartMeals = new HashSet<CartMeal>();
            ItemMeals = new HashSet<ItemMeal>();
        }

        public int MealId { get; set; }
        public string? MealName { get; set; }
        public string? MealDescription { get; set; }
        public int? ImageId { get; set; }
        public int? CategoryId { get; set; }
        public bool? Availability { get; set; }
        public double? Price { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public int? LastModifiedUserId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Image? Image { get; set; }
        public virtual User? LastModifiedUser { get; set; }
        public virtual ICollection<CartMeal> CartMeals { get; set; }
        public virtual ICollection<ItemMeal> ItemMeals { get; set; }
    }
}
