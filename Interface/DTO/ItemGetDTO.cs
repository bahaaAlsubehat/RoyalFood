using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class ItemGetDTO
    {
        public string itemId {  get; set; }
        public string itemName { get; set; }
        public string itemNameAr { get; set; }
        public string decreptionItem { get; set; }
        public string decreptionItemAr { get; set; }

        public string category { get; set; }
        public string price { get; set; }
        public string availability { get; set; }
        public string lastmodification { get; set; }
        public string lastusermodefied { get; set; }
        public List<ItemIngredGetDTO>? myingitget {get; set;}
        public List<ItemImageGetDTO>? myimagesitem { get; set;}

    }
}
