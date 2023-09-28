using Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class LogoutResponse
    {
        public int userid { get; set; }
        public int loginid { get; set;}
        public List<Cart> cartUser { get; set; }    
    }
}
