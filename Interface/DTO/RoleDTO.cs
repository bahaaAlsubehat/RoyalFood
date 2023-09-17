using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DTO
{
    public class RoleDTO
    {
        [Required]
        public string name {  get; set; }
        public string permission { get; set; }
    }
}
