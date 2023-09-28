using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class MealGet
    {
        public string? mealid {  get; set; }
        public string? name { get; set; }
        public string? nameAr { get; set; }
        public string? description { get; set; }
        public string? descriptionAr { get; set; }
        public string? image { get; set; }
        public string? category { get; set; }
        public string? price { get; set; }
        public string? availability { get; set; }
        public string? lastmodificationdate { get; set; }
        public string? lastuseridmodified { get; set; }
        public List<MealItemGetDTO>? Mymealitem { get; set; } 
        public List<ImagesMealGetDTO>? myimagesmeal { get; set; }

    }
}
