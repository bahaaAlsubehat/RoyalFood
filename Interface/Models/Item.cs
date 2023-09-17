using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Item
    {
        public Item()
        {
            CartItems = new HashSet<CartItem>();
            IngredientItems = new HashSet<IngredientItem>();
            ItemMeals = new HashSet<ItemMeal>();
        }

        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescribtion { get; set; }
        public int? CategoryId { get; set; }
        public double? Price { get; set; }
        public bool? Availability { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public int? LastModifiedUserId { get; set; }
        public int? ImageId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Image? Image { get; set; }
        public virtual User? LastModifiedUser { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<IngredientItem> IngredientItems { get; set; }
        public virtual ICollection<ItemMeal> ItemMeals { get; set; }
    }
}
