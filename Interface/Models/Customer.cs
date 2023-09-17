using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string? UserName { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
