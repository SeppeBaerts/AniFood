using AniFoodNew.DataB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NotChecked))]
        private bool _checked;
        [ObservableProperty]
        private string _familyCode;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private string? _nickname;
        [ObservableProperty]
        private string _firstName;
        [ObservableProperty]
        private string _lastName;
        [ObservableProperty]
        private string _familyName;

        public bool NotChecked => !Checked;

        [RelayCommand]
        public async Task Register()
        {
            string email = Email;
            string password = Password;
            string nickname = Nickname?? "";
            string firstName = FirstName;
            string lastName = LastName;
            string? familyCode;
            string? familyName;
            if (Checked)
            {
                familyCode = string.IsNullOrWhiteSpace(FamilyCode) ? null : FamilyCode;
                familyName = null;
            }
            else
            {
                familyName = string.IsNullOrWhiteSpace(FamilyName) ? null : FamilyName;
                familyCode = null;
            }


            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(nickname) && !string.IsNullOrWhiteSpace(Checked? familyCode : familyName))
            {
                RegisterModel register = new()
                {
                    Email = email,
                    Password = password,
                    NickName = nickname,
                    FirstName = firstName,
                    LastName = lastName,
                    FamilyCode = familyCode,
                    FamilyName = familyName
                };
                var answer =await ServerSender.RegisterUser(register);
                if (answer.IsSuccess)
                {
                    await Shell.Current.GoToAsync("///LoadingPage");
                }
            }
            else
            {
                var toast = Toast.Make("Please enter a username and password");
                await toast.Show();
            }
        }
    }
}
