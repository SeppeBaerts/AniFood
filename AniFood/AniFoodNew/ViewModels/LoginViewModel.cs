using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using CommunityToolkit.Maui.Core.Views;
using System.ComponentModel.DataAnnotations;
using AniFoodNew.DataB;

namespace AniFoodNew.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;

        [RelayCommand]
        public async Task Register()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        [RelayCommand]
        public async Task Login()
        {
            string username = Username;
            string password = Password;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                try
                {
                    var answer = await ServerGetter.GetUserAsync(username, password);
                    if (answer.IsSuccess)
                    {
                        await Shell.Current.GoToAsync("///LoadingPage");
                    }
                }
                catch
                {
                    Toast.Make("Something went wrong logging in. please try again later", CommunityToolkit.Maui.Core.ToastDuration.Long);
                }
            }
            else
            {
                await Toast.Make("Please enter a username and password").Show();
            }
        }
    }
}