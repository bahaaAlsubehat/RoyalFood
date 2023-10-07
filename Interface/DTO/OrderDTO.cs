using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class OrderDTO
    {
        public int cartid { get; set; }
       // public DateTime deliverytime { get; set; }
        public string deliveryaddress { get; set; }
        public int paymentid { get; set; }
        public string customernote { get; set; }
        public string ratingfeedback { get; set; }


    }
}
