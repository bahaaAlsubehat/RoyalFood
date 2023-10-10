using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItemMeals = new HashSet<CartItemMeal>();
            Orders = new HashSet<Order>();
        }

        public int CartId { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<CartItemMeal> CartItemMeals { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
