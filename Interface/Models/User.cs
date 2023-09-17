using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            CustomerServices = new HashSet<CustomerService>();
            Customers = new HashSet<Customer>();
            Items = new HashSet<Item>();
            Logins = new HashSet<Login>();
            Meals = new HashSet<Meal>();
        }

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public int? Age { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<CustomerService> CustomerServices { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Login> Logins { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
