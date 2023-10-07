using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class ItemIngredGetDTO
    {
        public int? itemid { get; set; }
        public string ingredientName { get; set; }
        public string ingredientNameAr { get; set; }
        public string unit { get; set; }
        public string qnty { get; set; }
        public string ingredientId { get; set; }

    }
}
