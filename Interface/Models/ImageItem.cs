using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class ImageItem
    {
        public int ImageItemId { get; set; }
        public int? ItemId { get; set; }
        public string? Path { get; set; }
        public bool? IsDefault { get; set; }

        public virtual Item? Item { get; set; }
    }
}
