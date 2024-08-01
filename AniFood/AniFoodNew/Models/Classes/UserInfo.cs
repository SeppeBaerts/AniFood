using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public List<Guid> FamilyIds { get; set; }
        public string NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserEmail { get; set; }

    }
}
