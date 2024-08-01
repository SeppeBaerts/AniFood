using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    /// <summary>
    /// This will be used for family user (this way the app will not get any 'sensitive' information)
    /// </summary>
    public class UserInfoModel
    {
        //Hier kan in principe nog een imageSource aan toevoegen.
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public bool IsFamilyHead { get; set; }
    }
}
