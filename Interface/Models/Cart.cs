using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
            CartMeals = new HashSet<CartMeal>();
            Orders = new HashSet<Order>();
        }

        public int CartId { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<CartMeal> CartMeals { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
