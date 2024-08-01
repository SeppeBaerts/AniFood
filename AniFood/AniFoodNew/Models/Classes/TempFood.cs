using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    public class TempFood
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> Animals { get; set; }
        public Guid ParentFamily { get; set; }
        public string ImageUri { get; set; }
        public int Type { get; set; }
        public int Capacity { get; set; }
        public int CurrentCapacity { get; set; }
        public int NewBags { get; set; }
    }
}
