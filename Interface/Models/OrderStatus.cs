﻿using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int OrderStatusId { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
