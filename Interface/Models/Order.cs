using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? CartId { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderStatusId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? CustomerNotes { get; set; }
        public string? RatingandFeedback { get; set; }
        public string? DelivaryAddress { get; set; }
        public int? PaymentId { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual OrderStatus? OrderStatus { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
