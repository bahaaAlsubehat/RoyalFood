using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class IngredientItem
    {
        public int IngridientItemId { get; set; }
        public int? ItemId { get; set; }
        public int? IngredientId { get; set; }
        public double? Quantity { get; set; }

        public virtual Ingredient? Ingredient { get; set; }
        public virtual Item? Item { get; set; }
    }
}
