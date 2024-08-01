using AniFoodNew.DataB;
using AniFoodNew.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels.ChangeViewModels
{
    public partial class ChangeAnimalViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime _CurrentDate = DateTime.Now;

        [ObservableProperty]
        private Animal _animal;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Animal))]
        private bool _changedPicture;

        string oldImageUri;
        [RelayCommand]
        public async Task TakePicture()
        {
            var result = await MediaPicker.CapturePhotoAsync();
            ProcessPicture(result);
        }
        [RelayCommand]
        public async Task ChoosePicture()
        {
            var result = await MediaPicker.PickPhotoAsync();
            ProcessPicture(result);
        }
        private void ProcessPicture(FileResult? result)
        {
            if (result != null)
            {
                if (!ChangedPicture)
                    oldImageUri = Animal.ImageUri;

                ChangedPicture = true;
                Animal.ImageUri = result.FullPath;
            }
        }
        [RelayCommand(CanExecute = nameof(ChangedPicture))]
        public void RemovePicture()
        {
            Animal.ImageUri = oldImageUri;
            ChangedPicture = false;
        }
        [RelayCommand]
        public async Task ChangeAnimal()
        {
            if (Animal.Name != null && Animal.ImageUri != null && Animal.FoodAmountPerDay>0 && Animal.FoodTimesPerDay>0)
            {
                if (ChangedPicture)
                    Animal.ImageUri = await ServerSender.UploadPhotoAsync(Animal.ImageUri, "Animal/UploadImage")??"Unknown.png";
                TempAnimal temp = new()
                {
                    Id = Animal.Id,
                    Name = Animal.Name,
                    ImageUri = Animal.ImageUri.Replace($"{BaseServerConnector.BaseApiLink}api/Animal/GetImage/",null),
                    FoodAmountPerDay = Animal.FoodAmountPerDay,
                    FoodTimesPerDay = Animal.FoodTimesPerDay,
                    Birthday = Animal.Birthday,
                    Notes = Animal.Notes,
                    Breed = Animal.Breed,
                    FoodId = Guid.Empty,
                    MainFamilyId = Guid.Empty,
                    LastTimeFed = Animal.LastTimeFed,
                    TimesFed = Animal.TimesFed
                };
                bool hasChanged = await ServerSender.ChangeAnimal(temp);
                if (hasChanged)
                    await Shell.Current.GoToAsync("///LoadingPage");
                else
                    await Toast.Make("Something went wrong trying to change the animal.").Show();
            }
            else
                await Toast.Make("Please fill in all the fields.").Show();
        }
    }
}
