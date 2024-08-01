using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models
{
    public class RegisterModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set;}
        public string? FamilyCode { get; set; }
        public string? FamilyName { get; set; }
    }
}
