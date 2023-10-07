using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class ImagesMealGetDTO
    {
        public int mealid { get; set; }
        public string imageid { get; set; }
        public string imagepath { get; set; }
        public string isdefault { get; set; }
    }
}
