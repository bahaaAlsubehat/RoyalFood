using Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class OrderItemandItemDTO
    {
        public int orderid { get; set; }
        public int cartitmealid { get; set; }
        //public Item item { get; set; }
        //public Meal meal { get; set; }
        public int? itemid { get; set; }
        public int? mealid { get; set; }
        public string? itemname { get; set; }
        public string? mealname { get; set; }
        public string? qnty { get; set; }
        public string? netprice { get; set; }
    }
}
