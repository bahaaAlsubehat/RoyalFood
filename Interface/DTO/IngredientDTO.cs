using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class IngredientDTO
    {
        public string ingName {  get; set; }    
        public string ingDescription { get; set; }
        public string unit { get; set; }

        public int imageId { get; set; }
        public bool isactive { get; set; }

    }
}
