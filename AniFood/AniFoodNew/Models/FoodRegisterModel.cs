using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models
{
    public class FoodRegisterModel
    {
        public string Name { get; set; }
        public string? ImageUri { get; set; }
        public Guid ParentFamily { get; set; }
        public int Capacity { get; set; }
        /// <summary>
        /// This will default to the capacity if not specified
        /// </summary>
        public int? CurrentCapacity { get; set; }
    }
}
