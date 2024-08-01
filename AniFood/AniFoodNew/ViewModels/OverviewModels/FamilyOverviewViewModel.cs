using AniFoodNew.Controls.Popups;
using AniFoodNew.DataB;
using AniFoodNew.Models.Classes;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class FamilyOverviewViewModel : BaseViewModel
    {
        private IGeolocation geoLocation;

        [ObservableProperty]
        private ObservableCollection<FullFamily> _famillies = new(MainUser.Families);
        [ObservableProperty]
        private FullFamily? _selectedFamily;

        [ObservableProperty]
        private bool _canSetFamilyLocation = Preferences.Get(AutoSwitchFamLocation, false);

        [ObservableProperty]
        private ObservableCollection<UserInfoModel>? _famUsers = new();

        [ObservableProperty]
        private bool _isFamilyHead;

        [ObservableProperty]
        private bool _isFamilySelected;

        public FamilyOverviewViewModel(IGeolocation location)
        {
            geoLocation = location;
        }


        [RelayCommand]
        public void SelectFamily(FullFamily family)
        {
            SelectedFamily = SelectedFamily == family ? null : family;
        }
        partial void OnSelectedFamilyChanged(FullFamily? value)
        {
            if (value != null)
            {
                FamUsers = new(value.SmallUsers);
                IsFamilyHead = value.FamilyHeadId == MainUser.UserId;
                IsFamilySelected = true;
            }
            else
            {
                FamUsers = null;
            }
        }

        [RelayCommand(CanExecute = nameof(IsFamilyHead))]
        public async Task RemoveFromFamily(UserInfoModel user)
        {
            if (user.Id != MainUser.UserId)
            {
                if (await ServerSender.DeleteUserFromFamily(SelectedFamily.FamilyId, user.Id))
                {
                    await Toast.Make($"{user.NickName} has been removed from the family.").Show();
                    MainUser.Families.Find(f => f.FamilyId == SelectedFamily.FamilyId).SmallUsers.Remove(user);
                    FamUsers.Remove(user);
                }
                else
                {
                    await Toast.Make("Something went wrong trying to remove the user from the family.").Show();
                }
            }
            else
            {
                await Toast.Make("You can't remove yourself from the family, use the Leave button instead.").Show();
            }
        }
        [RelayCommand(CanExecute = nameof(CanSetFamilyLocation))]
        public async Task SetFamilyLocation()
        {
            if (SelectedFamily == null)
            {
                await Toast.Make("You must select a family first!").Show();
                return;
            }

            try
            {
                Location? location = await GetCurrentLocation(geoLocation);
                if (location != null)
                {
                    Preferences.Set($"{SelectedFamily.FamilyId}Loc", JsonConvert.SerializeObject(location));
                    await Toast.Make("Location successfully configured", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                }
            }
            catch
            {
                await Toast.Make("Something went wrong trying to get your location.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
            }
        }

        [RelayCommand(CanExecute = nameof(IsFamilySelected))]
        public async Task ShowFamCode()
        {
            if (SelectedFamily == null)
            {
                await Toast.Make("You must select a family first").Show();
                return;
            }

            await Shell.Current.CurrentPage.ShowPopupAsync(new MainPopupPage("Famcode",
               $"Family code: {SelectedFamily.FamilyCode}",
               true,
               "Ok",
               null,
               "Copy",
               new RelayCommand(CopyText)));
        }
        public void CopyText()
        {
            Clipboard.SetTextAsync(SelectedFamily.FamilyCode);
        }

        [RelayCommand(CanExecute = nameof(IsFamilySelected))]
        public async Task LeaveFamily()
        {
            if (SelectedFamily == null || Famillies.Count == 1)
                return;

            bool comformation = await Shell.Current.CurrentPage.DisplayAlert("Leaving Family", $"Are you sure you want to leave the {SelectedFamily.FamilyName} family?", "Yes", "No");
            if (!comformation)
                return;

            if (await ServerSender.LeaveFamily(SelectedFamily.FamilyId))
            {
                await Shell.Current.GoToAsync("///LoadingPage");
            }
            else
            {
                await Toast.Make("Something went wrong leaving the family. Please try again later.").Show();
            }
        }

        public void Refresh()
        {
            Famillies = new(MainUser.Families);
            SelectedFamily = null;
        }
        [RelayCommand]
        public async Task DeleteFamily()
        {
            if (IsFamilyHead)
            {
                bool acceptDelete = await Shell.Current.CurrentPage.DisplayAlert("Delete family", $"Are you sure you want to delete the {SelectedFamily.FamilyName} family?", "Yes", "No");
                if (acceptDelete && MainUser.Families.Count > 1)
                {
                    var issuccess = await ServerSender.DeleteFamilyAsync(SelectedFamily.FamilyId);
                    if (issuccess)
                    {
                        await Toast.Make("Succesfully deleted family").Show();
                        MainUser.Families.Remove(SelectedFamily);
                        Famillies.Remove(SelectedFamily);
                        Preferences.Default.Remove($"{SelectedFamily.FamilyId}Loc");
                        foreach (Animal ani in SelectedFamily.Animals)
                        {
                            Preferences.Default.Remove($"{ani.Id}NotId");
                        }
                        SelectedFamily = null;
                    }
                }
            }
            else
            {
                await Toast.Make("Only the family head can delete the family.").Show();
            }
        }

    }
}
