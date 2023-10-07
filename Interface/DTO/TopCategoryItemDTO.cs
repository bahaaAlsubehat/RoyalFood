using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class TopCategoryItemDTO
    {
        public int? itemid { get; set; }
        public int? categoryid { get; set; }
        public string? categoryname { get; set; }
        public int? qnty { get; set; }
    }
}
