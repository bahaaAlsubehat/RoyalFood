using Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class TopItemDTO
    {
        public int? itemid { get; set; } 
        public string? itemname { get; set; }
        public int? qny { get; set; }
    }
}
