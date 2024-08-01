using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models
{
    public class AnimalRegisterModel
    {
        public string Name { get; set; }
        public string? ImageUri { get; set; }
        public Guid? FoodId { get; set; }
        public Guid? MainFamilyId { get; set; }
        public int FoodAmountPerDay { get; set; }
        public int FoodTimesPerDay { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Notes { get; set; }
        public string? Breed { get; set; }
    }
}
