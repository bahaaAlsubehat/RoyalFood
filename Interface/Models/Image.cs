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
        }

        public int ImageId { get; set; }
        public string? Path { get; set; }
        public bool? IsDefault { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
