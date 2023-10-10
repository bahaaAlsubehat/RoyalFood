using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class ItemsMealsOnOrderDTO
    {
        //public int? carid { get; set; }
        //public int? cartitemmealid { get; set; }
        public int? itemid { get; set; }
        public int? mealid { get; set; }
        public int? qntyitem { get; set; }
        public int? qntymeal { get; set; }

        //public float? netprice { get; set; }
    }
}
