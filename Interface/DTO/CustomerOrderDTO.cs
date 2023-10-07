using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class CustomerOrderDTO
    {
        public int userid { get; set; }
        public string ordernumber { get; set; }
        public string? orderdate { get; set; }
        public string?  delivarydate { get; set; }
        public string? totalprice { get; set; }
        public string? orderstatus { get; set; }


    }
}
