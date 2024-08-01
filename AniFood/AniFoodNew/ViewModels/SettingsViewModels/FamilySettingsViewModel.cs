using AniFoodNew.Models.Classes;
using AniFoodNew.Views.AddPages;
using AniFoodNew.Views.Overview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels.SettingsViewModels
{
    public partial class FamilySettingsViewModel : BaseViewModel
    {
        IGeolocation geoLocation;
        public FamilySettingsViewModel(IGeolocation location)
        {
            geoLocation = location;
        }
        [ObservableProperty]
        private bool _useFamilyLocation = Preferences.Default.Get(AutoSwitchFamLocation, false);
        partial void OnUseFamilyLocationChanged(bool value)
        {
            Preferences.Default.Set(AutoSwitchFamLocation, value);
        }
        [RelayCommand(CanExecute = nameof(UseFamilyLocation))]
        public async Task SetFamilyLocation(FullFamily family)
        {
            try
            {
                Location? location = await GetCurrentLocation(geoLocation);
                if (location != null)
                {
                    Preferences.Default.Set($"{family.FamilyId}Loc", JsonConvert.SerializeObject(location));
                    await Toast.Make("Location successfully configured", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                }
            }
            catch
            {
                await Toast.Make("Something went wrong trying to get your location.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
            }
        }
        [RelayCommand]
        public async Task LeaveFamily()
        {
            if (MainUser.Families.Count == 1)
            {
                await Toast.Make("You can't leave a family since you only have one.").Show();
                return;
            }

            await Shell.Current.CurrentPage.DisplayActionSheet("What family do you want to leave", "Cancel", null, MainUser.Families.Select(f => f.FamilyName).ToArray())
                .ContinueWith(async (task) =>
                {
                string? result = await task;
                if (result == "Cancel")
                    return;
                Guid familyId = MainUser.Families.First(f => f.FamilyName == result).FamilyId;
                Preferences.Default.Remove($"{familyId}Loc");
                var answer = await ServerSender.LeaveFamily(familyId);
                    if (answer)
                        await Shell.Current.GoToAsync("///LoadingPage");
                    else
                        await Toast.Make("Something went wrong leaving the family. Please try again later.").Show() ;
                });
        }       
        [RelayCommand]
        public async Task AddFamily()
        {
            await Shell.Current.CurrentPage.DisplayPromptAsync("Add family", "Please enter the family code", "Ok", "Cancel", "Family code").ContinueWith(async (task) =>
            {
                string? result = await task;
                if (result != null)
                {
                    var answer = await ServerSender.AddUserToFamilyCode(result);
                    if (answer)
                    {
                        await Shell.Current.GoToAsync("///LoadingPage");
                    }
                    else
                    {
                        await Toast.Make("Something went wrong adding you to the family. Please try again later.").Show();
                    }
                }
            });
        }

        [RelayCommand]
        public async Task GoToOverview()
        {
            await Shell.Current.GoToAsync(nameof(FamilyOverviewPage));
        }
        [RelayCommand]
        public async Task GoToCreate()
        {
            await Shell.Current.GoToAsync(nameof(CreateFamilyPage));
        }
    }
}
