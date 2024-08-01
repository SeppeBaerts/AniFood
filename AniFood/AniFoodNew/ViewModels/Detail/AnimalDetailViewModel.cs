using AniFoodNew.Models.Classes;
using AniFoodNew.Views.Change;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels.Detail
{
    public partial class AnimalDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Animal _animal;

        [RelayCommand]
        public async Task FeedAnimal()
        {
            var answer = await ServerSender.FeedAnimal(Animal.Id);
            if (answer)
            {
                Animal.TimesFed++;
                Animal.LastTimeFed = DateTime.Now;
                IToast t = CommunityToolkit.Maui.Alerts.Toast.Make("Animal has been fed.");
                await t.Show();
            }
            else
                await Toast.Make("Something went wrong trying to feed the animal.").Show();
        }
        [RelayCommand]
        public async Task RemoveAnimal()
        {
            if (Animal.MainFamily.Animals.Count == 1)
            {
                await Toast.Make("You can't delete the last animal of a family.").Show();
                return;
            }
            bool hasDeleted = await ServerSender.DeleteAnimal(Animal.Id);
            if (hasDeleted)
                await Shell.Current.GoToAsync("///LoadingPage");
            else
                await Toast.Make("Something went wrong trying to delete the animal.").Show();
        }
        [RelayCommand]
        public async Task ChangeAnimal()
        {
            var navParams = new Dictionary<string, object>
            {
                {"animal", Animal },
            };
            await Shell.Current.GoToAsync(nameof(ChangeAnimalPage), navParams);
        }
        [RelayCommand]
        public async Task AddFood()
        {
            if (Animal.MainFamily.Foods.Count == 0)
            {
                await Toast.Make("Create food first.", ToastDuration.Long, 28).Show();
                return;
            }
            string answer = await Shell.Current.CurrentPage.DisplayActionSheet("Select food", "Cancel", null, Animal.MainFamily.Foods.Select(f => f.Name).ToArray());
            if (answer == "Cancel")
                return;
            Food food = Animal.MainFamily.Foods.Find(f => f.Name == answer);
            bool isSuccess = await ServerSender.AddFoodToAnimal(Animal.Id,food.Id);
            if (isSuccess)
            {
                food.Animals.Add(Animal);
                Animal.Food = food;
                await Toast.Make("Food has been added.").Show();
            }
            else
                await Toast.Make("Something went wrong trying to add the food.").Show();
        }
    }
}
