using AniFoodNew.DataB;
using AniFoodNew.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class AddAnimalViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime _currentDate = DateTime.Now;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasSelectedFamily))]
        [NotifyCanExecuteChangedFor(nameof(CreateAnimalCommand))]
        private FullFamily? _selectedFamily;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NotSentAnimInfoBack),nameof(HasSelectedFamily))]
        [NotifyCanExecuteChangedFor(nameof(CreateAnimalCommand))]
        private bool _sentAnimalInfoBack = false;

        [ObservableProperty]
        private Food? _selectedFood;

        public bool NotSentAnimInfoBack => !SentAnimalInfoBack;

        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string? _imageUri;
        [ObservableProperty]
        private string _foodAmountPerDay;
        [ObservableProperty]
        private string _foodTimesPerDay;
        [ObservableProperty]
        private DateTime _birthDay = DateTime.Today;
        [ObservableProperty]
        private string _breed;
        [ObservableProperty]
        private string _notes;

        [ObservableProperty]
        private ObservableCollection<FullFamily> _families = new(MainUser.Families);

        [ObservableProperty]
        private ObservableCollection<Food>? _foods;

        bool HasSelectedFamily => SentAnimalInfoBack || SelectedFamily != null;

        [RelayCommand(CanExecute = nameof(HasSelectedFamily))]
        public async Task CreateAnimal()
        {
            if (int.TryParse(FoodAmountPerDay, out int foodAmount)
                && int.TryParse(FoodTimesPerDay, out int foodTimes)
                && HasValidAnswers())
            {
                AnimalRegisterModel registerModel = new()
                {
                    Name = Name,
                    ImageUri = ImageUri,
                    FoodId = SelectedFood == null ? Guid.Empty : SelectedFood.Id,
                    MainFamilyId = SelectedFamily.FamilyId,
                    FoodAmountPerDay = foodAmount,
                    FoodTimesPerDay = foodTimes,
                    Birthday = BirthDay,
                    Notes = Notes,
                    Breed = Breed
                };
                var response = await ServerSender.RegisterAnimalAsync(registerModel);
                if (response.IsSuccess)
                {
                    await Shell.Current.GoToAsync("///LoadingPage");
                }
                else
                {
                    Toast.Make("Something went wrong creating the animal. please try again later", CommunityToolkit.Maui.Core.ToastDuration.Long);
                }
            }
            else
            {
                Toast.Make("Please fill in all the fields correctly.", CommunityToolkit.Maui.Core.ToastDuration.Long);
            }
        }

        [RelayCommand]
        public async Task SentAnimalBack()
        {
            if (int.TryParse(FoodAmountPerDay, out int foodAmount)
               && int.TryParse(FoodTimesPerDay, out int foodTimes)
               && !string.IsNullOrWhiteSpace(Name))
            {
                AnimalRegisterModel registerModel = new()
                {
                    Name = Name,
                    ImageUri = ImageUri,
                    FoodId = Guid.Empty,
                    MainFamilyId = Guid.Empty,
                    FoodAmountPerDay = foodAmount,
                    FoodTimesPerDay = foodTimes,
                    Birthday = BirthDay,
                    Notes = Notes,
                    Breed = Breed
                };
                var navParams = new Dictionary<string, object>
                {
                    {"animalModel", registerModel },
                };
                await Shell.Current.GoToAsync("..", navParams);
            }
        }
        [RelayCommand]
        public void SelectFamily(FullFamily family)
        {
            SelectedFamily = SelectedFamily == family? null : family;
        }

        [RelayCommand]
        public void SelectFood(Food food)
        {
            SelectedFood = SelectedFood == food ? null : food;
        }

        partial void OnSelectedFamilyChanged(FullFamily? value)
        {
                SelectedFood = null;
            if (value == null || value.Foods == null)
                Foods = null;
            else
                Foods = new(value.Foods);
        }

        [RelayCommand]
        public async Task TakePicture()
        {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                ImageUri = result.FullPath;
            }
        }
        [RelayCommand]
        public async Task ChoosePicture()
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                ImageUri = result.FullPath;
            }
        }

        public bool HasValidAnswers()
        {
            return (!string.IsNullOrWhiteSpace(Name))
                && (SelectedFamily != null);
        }

    }
}
