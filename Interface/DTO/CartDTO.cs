using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class CartDTO
    {
        public int? itemid { get; set; }
        public int? mealid { get; set; }
        public int? qnty { get; set; }
        public float? price { get; set; }
        public int? userid { get; set; }

    }
}
