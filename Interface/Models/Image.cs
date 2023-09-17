using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Image
    {
        public Image()
        {
            Categories = new HashSet<Category>();
            Ingredients = new HashSet<Ingredient>();
            Items = new HashSet<Item>();
            Meals = new HashSet<Meal>();
        }

        public int ImageId { get; set; }
        public string? Path { get; set; }
        public bool? IsDefault { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
