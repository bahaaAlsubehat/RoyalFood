using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class CartItemMeal
    {
        public int CartItemMealId { get; set; }
        public int? CartId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public double? NetPrice { get; set; }
        public int? MealId { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Item? Item { get; set; }
        public virtual Meal? Meal { get; set; }
    }
}
