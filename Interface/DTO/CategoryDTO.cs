using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class CategoryDTO
    {
        public string categoryName {  get; set; }
        public string categoryNameAr { get; set; }
        public string description { get; set; }
        public string descriptionAr { get; set; }

        public bool isactive { get; set; }
        public int imageId { get; set; }
    }
}
