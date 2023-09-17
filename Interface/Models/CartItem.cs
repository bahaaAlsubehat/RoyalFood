using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class CartItem
    {
        public int CartItemId { get; set; }
        public int? CartId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public double? NetPrice { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Item? Item { get; set; }
    }
}
