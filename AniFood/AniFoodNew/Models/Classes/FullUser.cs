using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    public class FullUser
    {
        public Guid UserId { get; set; }
        public List<FullFamily> Families { get; set; }
        public string NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserEmail { get; set; }
        public bool LoadFromUserInfo(UserInfo userInfo)
        {
            UserId = userInfo.UserId;
            Families = new();
            NickName = userInfo.NickName;
            FirstName = userInfo.FirstName;
            LastName = userInfo.LastName;
            UserEmail = userInfo.UserEmail;
            return true;
        }
    }
}
