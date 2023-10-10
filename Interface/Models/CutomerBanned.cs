using System;
using System.Collections.Generic;

namespace Interface.Models
{
    public partial class CutomerBanned
    {
        public int CustomerBannedId { get; set; }
        public bool? BannedAction { get; set; }
        public int? LoginId { get; set; }
    }
}
