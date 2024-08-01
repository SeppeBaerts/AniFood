using AniFoodNew.Controls.Popups;
using AniFoodNew.Models.Classes;
using AniFoodNew.Views.Detail;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Compatibility;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool _canFeedOnSwipe;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private ObservableCollection<FullFamily> _families = new ObservableCollection<FullFamily>(MainUser.Families);
        [ObservableProperty]
        private ObservableCollection<Animal> _animals = new ObservableCollection<Animal>(AllAnimals);
        [ObservableProperty]
        private ObservableCollection<Food> _foods = new ObservableCollection<Food>(AllFoods);
        [ObservableProperty]
        private FullFamily? _selectedFamily;

        private Button? AllButton;
        private Button? FavButton;
        private IGeolocation _geoLocation;
#if IOS || ANDROID
        private INotificationService _notificationService;
#endif
        public MainViewModel(IGeolocation location)
        {
            _geoLocation = location;
#if IOS || ANDROID
            _notificationService = LocalNotificationCenter.Current;
#endif
        }
        private void ResetButtons()
        {
            VisualStateManager.GoToState(AllButton, VisualStateManager.CommonStates.Normal);
            VisualStateManager.GoToState(FavButton, VisualStateManager.CommonStates.Normal);
        }
        partial void OnSelectedFamilyChanged(FullFamily? value)
        {
            ResetButtons();
            if (value != null)
            {
                Animals = new ObservableCollection<Animal>(value.Animals);
                Foods = new ObservableCollection<Food>(value.Foods);
            }
        }
        [RelayCommand]
        public void ChangeFilterWithFamily(FullFamily family)
        {
            SelectedFamily = family;
        }
        [RelayCommand]
        public void ChangeFilter(string filter)
        {
            SelectedFamily = null;
            ResetButtons();
            Foods = new ObservableCollection<Food>(AllFoods); //food cannot be favorite
            VisualStateManager.GoToState(filter == "All" ? AllButton : FavButton, "Selected");
            Animals = new ObservableCollection<Animal>(filter == "All" ? AllAnimals : AllAnimals.Where(a => Favorites.Contains(a.Id)));
        }
        public void SetButtons(Button allButton, Button favButton)
        {
            AllButton = allButton;
            FavButton = favButton;
        }
        public async Task Init()
        {
#if IOS || ANDROID
            var notifications = await _notificationService.GetPendingNotificationList();
            if (Preferences.Default.Get(SendFeedNotificationLocation, false))
            {
                foreach (var animal in AllAnimals)
                {
                    int notificationId = Preferences.Default.Get($"{animal.Id}NotId", 0);
                    if (notificationId == 0)
                        continue;
                    var notification = notifications.FirstOrDefault(n => n.NotificationId == notificationId);
                    if (notification == null)
                        continue;
                    if (notification != null)
                        notification.Schedule.NotifyTime = notification.Schedule.NotifyTime?.AddDays(1);
                    await _notificationService.Show(notification);
                    Preferences.Default.Set($"{animal.Id}NotId", notificationId);
                    notificationId++;
                }
            }
            if (Preferences.Default.Get(SendFoodNotificationLocation, false))
            {
                foreach (var food in AllFoods)
                {
                    int notificationId = Preferences.Default.Get($"{food.Id}NotId", 0);
                    if (notificationId == 0)
                        continue;

                    var notification = notifications.FirstOrDefault(n => n.NotificationId == notificationId);

                    if (notification == null)
                        continue;
                    notification.Schedule.NotifyTime = DateTime.Now.AddDays(food.DaysLeftInt - 1);
                    await _notificationService.Show(notification);
                    Preferences.Default.Set($"{food.Id}NotId", notificationId);
                    notificationId++;
                }
            }
#endif


            if (Preferences.Default.Get(AutoSwitchFamLocation, false))
            {
                Location? currentLocation = await GetCurrentLocation(_geoLocation);
                if (currentLocation != null)
                {
                    foreach (var family in Families)
                    {
                        string locationString = Preferences.Get($"{family.FamilyId}Loc", "");
                        Location? loc = JsonConvert.DeserializeObject<Location>(locationString);
                        if (loc != null && loc.CalculateDistance(currentLocation, DistanceUnits.Kilometers) * 1000 < loc.Accuracy + currentLocation.Accuracy)
                        {
                            SelectedFamily = family;
                            return;
                        }
                    }
                }
            }

            ChangeFilter("All");
        }
        [RelayCommand]
        public void Refresh()
        {
            if (Families.ToList() != MainUser.Families)
                Families = new ObservableCollection<FullFamily>(MainUser.Families);
            if (Foods.ToList() != AllFoods)
                Foods = new ObservableCollection<Food>(AllFoods);
            if (Animals.ToList() != AllAnimals)
                Animals = new ObservableCollection<Animal>(AllAnimals);

            SelectedFamily = null;
            VisualStateManager.GoToState(AllButton, "Selected");
            IsRefreshing = false;
        }
        public void OnAppearing()
        {
            CanFeedOnSwipe = Preferences.Default.Get(SwipeToFeedLocation, true);
        }

        [RelayCommand]
        public async static Task AddAnimal()
        {
            await Shell.Current.GoToAsync("AddAnimalPageStacked");
        }
        [RelayCommand]
        public async Task AddFood()
        {
            await Shell.Current.GoToAsync("AddFoodPageStacked");
        }

        [RelayCommand]
        public async Task FeedAnimal(Animal ani)
        {
            bool answer = await ServerSender.FeedAnimal(ani.Id);
            if (answer)
            {
                ani.TimesFed++;
                ani.LastTimeFed = DateTime.Now;
                if (ani.Food != null)
                    ani.Food.CurrentCapacity -= (int)Math.Round((double)ani.FoodAmountPerDay / ani.FoodTimesPerDay);

                await Toast.Make("Animal has been fed").Show();
            }
            else
                await Toast.Make("Something went wrong.").Show();
        }

        [RelayCommand]
        public void PlusFood(Food food)
        {
            food.NewBags++;
        }
        [RelayCommand]
        public async Task MinusFood(Food food)
        {
            if (food.NewBags > 0)
                food.NewBags--;
            else if (food.NewBags == 0)
            {
                bool answer = await Shell.Current.CurrentPage.DisplayAlert("Delete this food?", "You are going to delete this food-item, do you want to continue?", "Yes", "No");
                if (answer)
                {
                    if (await ServerSender.DeleteFoodAsync(food.Id))
                    {
                        Foods.Remove(food);
                        await Toast.Make("Food Succesfully deleted").Show();
                        await Shell.Current.GoToAsync("///LoadingPage");
                    }
                    else
                    {
                        await Toast.Make("Something went wrong deleting the food").Show();
                    }
                }
            }
        }

        [RelayCommand]
        public async Task Details(object detailObject)
        {
            if (detailObject is Animal animal)
            {
                var navParams = new Dictionary<string, object>
                {
                    {"animal", animal },
                };
                await Shell.Current.GoToAsync(nameof(AnimalDetailPage), navParams);
            }
        }
        [RelayCommand]
        public void ToggleFavorite(Animal animal)
        {
            if (Favorites.Contains(animal.Id))
                RemoveFavorite(animal);
            else
                AddFavorite(animal);
        }
        public async Task OnDissapearing()
        {
            var allFoods = AllFoods;
            foreach (Food food in Foods)
            {
                if (food.NewBags == allFoods.Find(f => f.Id == food.Id)?.NewBags)
                    continue;
                await ServerSender.UpdateFoodBagsAsync(food.Id, food.NewBags);
            }
        }

    }
}
