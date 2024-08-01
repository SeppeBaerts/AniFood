using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    public class TempAnimal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? FoodId { get; set; }
        public Guid MainFamilyId { get; set; }
        public string ImageUri { get; set; }
        public int FoodAmountPerDay { get; set; }
        public int FoodTimesPerDay { get; set; }
        public int TimesFed { get; set; }
        public DateTime? LastTimeFed { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Notes { get; set; }
        public string? Breed { get; set; }

    }
}
