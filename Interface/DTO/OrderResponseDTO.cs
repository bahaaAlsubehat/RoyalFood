using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class OrderResponseDTO
    {
        public string ordernumber { get; set; }
        public string customername { get; set; }
        public string customerphone { get; set; }
        public string orderDate { get; set; }
        public string delivarydate { get; set; }
        public string orderstatus { get; set; }
        public string totalprice { get; set; }
        public List<OrderItemandItemDTO>? orderitmlDTO { get; set; }
    }
}
