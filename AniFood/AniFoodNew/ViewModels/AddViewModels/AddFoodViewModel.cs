using AniFoodNew.DataB;
using AniFoodNew.Models.Classes;
using AniFoodNew.Views.SettingsPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class AddFoodViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _imageUri;
        [ObservableProperty]
        private string _capacity;
        [ObservableProperty]
        private ObservableCollection<FullFamily>? _fullFamilies = new(MainUser.Families);
        [ObservableProperty]
        private ObservableCollection<Animal>? _familyAnimals = new();

        [ObservableProperty]
        private ObservableCollection<object>? _selectedAnimals;

        [ObservableProperty]
        private FullFamily? _selectedFamily;

        [RelayCommand]
        public void FamButton(FullFamily fam)
        {
            if (SelectedFamily != fam)
                SelectedFamily = fam;
            else
                SelectedFamily = null;
        }

        [RelayCommand]
        public void AnimButton(Animal anim)
        {
            SelectedAnimals ??= [];
            if (SelectedAnimals.Contains(anim))
            {
                if (SelectedAnimals.Count == 1)
                    SelectedAnimals = null;
                else
                    SelectedAnimals.Remove(anim);
            }
            else
                SelectedAnimals.Add(anim);
        }

        partial void OnSelectedFamilyChanged(FullFamily? value)
        {
            if (value != null)
            {
                SelectedAnimals = [];
                FamilyAnimals = new(value.Animals);
            }
            else
            {
                SelectedAnimals = null;
                FamilyAnimals = null;
            }
        }

        [RelayCommand]
        public void AnimalsSelectionChanged(IList<object> something)
        {
            SelectedAnimals = [];
            foreach (var animal in something)
            {
                if (animal is Animal ani)
                    SelectedAnimals.Add(ani);
            }
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
        [RelayCommand]
        public async Task CreateFood()
        {
            if (int.TryParse(Capacity, out int capacity) && HasValidAnswers())
            {

                List<Animal> bindingAnimals = SelectedAnimals == null? [] : SelectedAnimals.Cast<Animal>().ToList();

                FoodRegisterModel registerModel = new()
                {
                    Name = Name,
                    ImageUri = ImageUri,
                    Capacity = capacity,
                    ParentFamily = SelectedFamily.FamilyId,
                    CurrentCapacity = capacity
                };
                ServerAnswer<Food> response;

                if (bindingAnimals.Count > 0)
                    response = await ServerSender.RegisterFoodAsync(registerModel, bindingAnimals);
                else
                    response = await ServerSender.RegisterFoodAsync(registerModel);

                if (response.IsSuccess)
                {
                    await Shell.Current.GoToAsync("///LoadingPage");
                }
                else
                {
                    Toast.Make("Something went wrong adding the food. Please try again later.");
                }
            }
            else
            {
                Toast.Make("Please fill in all the fields.");
            }
        }
        public bool HasValidAnswers()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedFamily != null;
        }
    }
}
