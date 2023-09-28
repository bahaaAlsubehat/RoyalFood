using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class ItemDTO
    {
        public string itemName { get; set; }
        public string itemNameAr { get; set; }

        public string decreptionItem { get; set; }
        public string decreptionItemAr { get; set; }

        public int categoryId { get; set; }
        public float price { get; set; }
        public bool availability { get; set; }
        public DateTime lastmodification { get; set; }
        public int lastusermodefied { get; set; }

        public List<ItemIngredientDTO>? MyIngredients { get; set; }
        public List<ItemImageCUDTO>? itemimgcuDTO { get; set; }
        
    }
}
