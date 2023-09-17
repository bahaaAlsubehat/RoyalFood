using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class ItemMeal
    {
        public int ItemMealId { get; set; }
        public int? ItemId { get; set; }
        public int? MealId { get; set; }
        public double? Quantity { get; set; }

        public virtual Item? Item { get; set; }
        public virtual Meal? Meal { get; set; }
    }
}
