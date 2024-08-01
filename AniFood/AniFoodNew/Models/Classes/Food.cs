using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    public class Food : ObservableObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Animal> Animals { get; set; }
        public FullFamily ParentFamily { get; set; }
        public string ImageUri { get; set; }
        public int Type { get; set; }
        public int Capacity { get; set; }
        private int _currentCapacity;

        public int CurrentCapacity
        {
            get { return _currentCapacity; }
            set { SetProperty(ref _currentCapacity, value); }
        }
        private int _newBags;
        public int NewBags
        {
            get { return _newBags; }
            set { SetProperty(ref _newBags, value); }
        }

        private int TotalUseGrams
        {
            get
            {
                int totalUseGrams = 0;
                foreach (Animal ani in Animals)
                {
                    totalUseGrams += ani.FoodAmountPerDay;
                }
                return totalUseGrams;
            }
        }
        public int DaysLeftInt => TotalUseGrams == 0? -1 : (int)Math.Round((decimal)CurrentCapacity / TotalUseGrams);
        
        public string DaysLeft
        {
            get
            {
                if(TotalUseGrams == 0)
                {
                    return "∞";
                }
                int daysLeft = DaysLeftInt;
                if (daysLeft < 0)
                    return 0.ToString();
                return daysLeft.ToString();
            }
        }
        public string AnimalsUsing => $"{Animals.Count} Dog{(Animals.Count == 1? "" : "s")}";

    }
}
