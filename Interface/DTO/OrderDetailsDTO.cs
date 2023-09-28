using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class OrderDetailsDTO
    {
        public string  ordernumber { get; set; }
        public string orderdate { get; set; }
        public string totalprics { get; set; }
        public string orderstatus { get; set; }
        public string delivaryaddress { get; set; }
        public string customerrate { get; set; }
        public string customernote { get; set; }
        public string paymentmethod { get; set; }
    }
}
