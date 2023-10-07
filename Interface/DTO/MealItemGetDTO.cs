using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class MealItemGetDTO
    {
        public int? itemid { get; set; }
        public int? mealid { get; set; }
        public string? itemname { get; set; }
        public string? itemmealar { get; set; }
        public string? qnty { get; set; }
        public string? unit { get; set; }
    }
}
