using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class CustomerService
    {
        public int CustomerServiceId { get; set; }
        public int? UserId { get; set; }
        public string? Query { get; set; }

        public virtual User? User { get; set; }
    }
}
