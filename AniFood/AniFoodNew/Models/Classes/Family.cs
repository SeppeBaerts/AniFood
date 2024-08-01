using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    [Obsolete("Use FullFamily instead")]
    public class Family
    {
        public Guid FamilyId { get; set; }
        public string FamilyName { get; set; }
        public string ImageUri { get; set; }
        public List<Guid> UserIds { get; set; }
        public List<Guid> FoodIds { get; set; }
        public List<Guid> AnimalIds { get; set; }
        public Guid FamilyHeadId { get; set; }
    }
}
/*
 "familyId": "e7a5441a-28f6-4a33-a214-08dbeaad128e",
    "name": "Family1",
    "imageUri": "none.jpg",
    "userIds": [
      "89fa17b4-f3e9-4463-6428-08dbea9b1a03",
      "c1749790-7bb7-43af-c985-08dbee91b26b",
      "c8da9524-147c-4eb8-69d3-08dbeea38314"
    ],
    "foodIds": [
      "5baa56b7-19fa-4de4-77ca-08dbef6f6f6e",
      "369fbea6-b53f-4834-d74c-08dbf01f30ff"
    ],
    "animalIds": [
      "4039df0f-da48-4980-3023-08dbef3f2297"
    ],
    "familyHeadId": "89fa17b4-f3e9-4463-6428-08dbea9b1a03",
    "familyCode": {
      "codeId": "0N7FNU",
      "familyId": "e7a5441a-28f6-4a33-a214-08dbeaad128e"
    }
 */