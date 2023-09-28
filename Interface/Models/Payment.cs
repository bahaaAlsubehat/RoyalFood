using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }

        public int PaymentId { get; set; }
        public string? PaymentMethod { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
