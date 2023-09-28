using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class MealDTO
    {
        public string? mealname { get; set; }
        public string? mealnameAr { get; set; }
        public string? description { get; set; }
        public string? descriptionAr { get; set; }
        public int? categoryid { get; set; }
        public bool? availability { get; set; }
        public float price { get; set; }
        public int? lastusermodifiy { get; set; }
        public DateTime lastmodificationdate { get; set; }

        public List<ItemMealDTO> myitemmeal { get; set; }
        public List<ImageMealDTO> myimagesmeal { get; set; }    
    }
}
