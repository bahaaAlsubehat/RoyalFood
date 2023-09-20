using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class ForgetPasswordDTO
    {
        public string email {  get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
