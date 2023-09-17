using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
            Meals = new HashSet<Meal>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? ImageId { get; set; }

        public virtual Image? Image { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
