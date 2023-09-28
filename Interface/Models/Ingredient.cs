using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            IngredientItems = new HashSet<IngredientItem>();
        }

        public int IngredientId { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? Describtion { get; set; }
        public string? DescribtionAr { get; set; }
        public string? Unit { get; set; }
        public bool? IsActive { get; set; }
        public int? ImageId { get; set; }

        public virtual Image? Image { get; set; }
        public virtual ICollection<IngredientItem> IngredientItems { get; set; }
    }
}
