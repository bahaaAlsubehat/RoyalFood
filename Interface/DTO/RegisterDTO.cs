using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class RegisterDTO
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Addrss { get; set; }
        public string phone { get; set; }
        public bool gender { get; set; }
        public int age { get; set; }
        public int roleid { get; set; }
        public string profileimage { get; set; }

        public string password { get; set; }
        public string email { get; set; }
        //public bool isactive { get; set; }

        public string? username { get; set; }    
    }
}
