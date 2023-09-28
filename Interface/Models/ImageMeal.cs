using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class ImageMeal
    {
        public int ImageMealId { get; set; }
        public int? MealId { get; set; }
        public string? Path { get; set; }
        public bool? IsDefualt { get; set; }

        public virtual Meal? Meal { get; set; }
    }
}
