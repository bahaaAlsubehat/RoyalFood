using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class CustomersResponseDTO
    {
        public string? customernamefirst { get; set; }
        public string? customernamelast { get; set; }
        public string ? username { get; set; }  
        public string? phone { get; set; }
        public string? eamil { get; set; }
        public string? lastlogin { get; set; }   
        public string? address { get; set; }
        public List<CustomerOrderDTO?> customerorderlist { get; set; }
    }
}
