using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class RemoveItemsOrMealsFromOrderDTO
    {
       //public int Orderid { get; set; }
        //public int Cartid { get; set; }
        public float totalprice { get; set; }
        //public DateTime delivarydate { get; set; }
        public string deliverydarress { get; set; }
        //public int? itemid { get; set; }
        //public int? mealid { get; set; }
        //public int? qnty { get; set; }

        public List<ItemsMealsOnOrderDTO>? myitemsmealsinorder { get; set; }


    }
}
