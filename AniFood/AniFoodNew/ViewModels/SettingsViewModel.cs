using AniFoodNew.Controls.Popups;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Views;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private List<Theme> _themeList = Enum.GetValues(typeof(Theme)).Cast<Theme>().ToList();
        [ObservableProperty]
        private List<MainTheme> _mainThemeList = Enum.GetValues(typeof(MainTheme)).Cast<MainTheme>().ToList();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSetMainTheme))]
        private Theme _selectedTheme = GetCurrentTheme();
        [ObservableProperty]
        private MainTheme _selectedMainTheme = GetCurrentMainTheme();
        [ObservableProperty]
        private bool _showFoodInOverview = Preferences.Default.Get(ShowFoodLocation, true);
        [ObservableProperty]
        private bool _showFoodNotifications = Preferences.Default.Get(SendFoodNotificationLocation, false);
        [ObservableProperty]
        private bool _showFeedNotifications = Preferences.Default.Get(SendFeedNotificationLocation, false);
        public bool  CanSetMainTheme => SelectedTheme != Theme.HighContrast;

        private INotificationService _notificationService;
        private int notificationId = Preferences.Default.Get(HighestNotificationIdLocation, 0);
        public SettingsViewModel()
        {
            if(DeviceInfo.Platform != DevicePlatform.WinUI)
                _notificationService = LocalNotificationCenter.Current;
        }

        [RelayCommand]
        public void Logout()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
                _notificationService.CancelAll();

            LogoutUser();
        }

        partial void OnSelectedThemeChanged(Theme value)
        {
            SetTheme(value);
        }
        partial void OnSelectedMainThemeChanged(MainTheme value)
        {
            SetMainTheme(value);
        }
        partial void OnShowFoodInOverviewChanged(bool value)
        {
            Preferences.Default.Set(ShowFoodLocation, value);
        }
        /// <summary>
        /// Will only trigger when not in windows
        /// </summary>
        /// <param name="value"></param>
        partial void OnShowFeedNotificationsChanged(bool value)
        {
#if IOS || ANDROID
            Preferences.Default.Set(SendFeedNotificationLocation, value);
            if (!value)
            {
                foreach (var animal in AllAnimals)
                {
                    int notId = Preferences.Default.Get($"{animal.Id}NotId", 0);
                    _notificationService.Cancel(notId);
                }
            }
            if (value && DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                DateTime now = DateTime.Now;
                now.AddHours(18 - DateTime.Now.Hour);
                now.AddMinutes(60 - DateTime.Now.Minute);
                now.AddSeconds(60 - DateTime.Now.Second);
                foreach (var animal in AllAnimals)
                {
                    var request = new NotificationRequest
                    {
                        Title = "AniFood",
                        Description = $"{animal.Name} is almost empty!",
                        BadgeNumber = 1,
                        NotificationId = notificationId,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = now,
                            NotifyRepeatInterval = TimeSpan.FromDays(1)
                        }
                    };
                    _notificationService.Show(request);
                    Preferences.Default.Set($"{animal.Id}NotId", notificationId);
                    notificationId++;
                }
                Preferences.Default.Set(HighestNotificationIdLocation, notificationId);
            }
#endif
        }

        partial void OnShowFoodNotificationsChanged(bool value)
        {
#if IOS || ANDROID
            Preferences.Default.Set(SendFoodNotificationLocation, value);

            if (!value)
            {
                foreach (var food in AllFoods)
                {
                    int notId = Preferences.Default.Get($"{food.Id}NotId", 0);
                    _notificationService.Cancel(notId);
                }
                return;
            }
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                foreach (var food in AllFoods)
                {
                    var request = new NotificationRequest
                    {
                        Title = "AniFood",
                        Description = $"{food.Name} is almost empty!",
                        BadgeNumber = 1,
                        NotificationId = notificationId,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now.AddDays(food.DaysLeftInt - 1)
                        }
                    };
                    _notificationService.Show(request);
                    Preferences.Default.Set($"{food.Id}NotId", notificationId);
                    notificationId++;
                }
                Preferences.Default.Set(HighestNotificationIdLocation, notificationId);
            }
#endif
        }
    }
}
