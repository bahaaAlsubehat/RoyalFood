using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class CartMeal
    {
        public int CartMealId { get; set; }
        public int? CartId { get; set; }
        public int? MealId { get; set; }
        public int? Quantitiy { get; set; }
        public double? NetPrice { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Meal? Meal { get; set; }
    }
}
