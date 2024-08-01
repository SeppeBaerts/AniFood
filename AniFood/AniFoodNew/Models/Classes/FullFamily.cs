using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    public class FullFamily
    {
        public Guid FamilyId { get; set; }
        public string FamilyName { get; set; }
        public string ImageUri { get; set; }
        public List<Guid> UserIds { get; set; }
        public List<Food> Foods { get; set; }
        public List<Animal> Animals { get; set; }
        public Guid FamilyHeadId { get; set; }
        public string? FamilyCode { get; set; }
        public List<UserInfoModel>? SmallUsers { get; set; }

    }
}
