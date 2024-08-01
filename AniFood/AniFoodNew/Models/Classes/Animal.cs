using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.Models.Classes
{
    public class Animal : ObservableObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Food? Food { get; set; }
        public FullFamily MainFamily { get; set; }
        private string _imageUri;
        public string ImageUri
        {
            get { return _imageUri; }
            set { SetProperty(ref _imageUri, value);}
        }

        public int FoodAmountPerDay { get; set; }
        public int FoodTimesPerDay { get; set; }
        private int _timesFed;

        public int TimesFed
        {
            get { return _timesFed; }
            set { SetProperty(ref _timesFed, value);
                 FoodArray = [value, FoodTimesPerDay - value];
                Percentage = $"{value}/{FoodTimesPerDay}";
            }
        }

        public DateTime? LastTimeFed { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Notes { get; set; }
        public string? Breed { get; set; }

        private bool _isFavorite;

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { SetProperty(ref _isFavorite, value); }
        }

        public string AgeInYears
        {
            get
            {
                var now = DateTime.Today;
                var age = now.Year - Birthday?.Year ?? 0;
                if (now < Birthday?.AddYears(age))
                {
                    age--;
                }
                return age.ToString() + $"Year{(age > 1 ? "s" : "")}";
            }
        }
        private int[] _foodArray;

        public int[] FoodArray
        {
            get { return _foodArray; }
            set { SetProperty(ref _foodArray, value); }
        }
        private string _percentage;

        public string Percentage
        {
            get { return _percentage; }
            set { SetProperty(ref _percentage, value); }
        }
        public string LastTimeFedString =>LastTimeFed.HasValue? LastTimeFed.Value.ToString("HH:mm - dd/MM/yyyy") : "Not yet fed";
    }

}
