using AniFoodNew.DataB;
using AniFoodNew.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels.AddViewModels
{

    public partial class CreateFamilyViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _familyName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasModel), nameof(HasNoModel))]
        [NotifyCanExecuteChangedFor(nameof(CreateFamilyCommand))]
        private AnimalRegisterModel? _registerModel;

        public bool HasModel => RegisterModel != null;
        public bool HasNoModel => !HasModel;


        [RelayCommand(CanExecute = nameof(HasModel))]
        public async Task CreateFamily()
        {
            try
            {
                Guid famId = await ServerSender.CreateFamilyAndGetGuidAsync(new FamilyRegisterModel { Name = FamilyName });
                MainUser!.Families.Add(new FullFamily
                {
                    FamilyHeadId = MainUser.UserId,
                    FamilyId = famId,
                    FamilyName = FamilyName,
                    Animals = [],
                    Foods = [],
                });
                RegisterModel!.MainFamilyId = famId;
               ServerAnswer<Animal> response = await ServerSender.RegisterAnimalAsync(RegisterModel!);
                if (response.IsSuccess)
                {
                    await Shell.Current.GoToAsync("///LoadingPage");
                }
                else
                    await Toast.Make("Something went wrong trying to create the family.").Show();
            }
            catch
            {
                await Toast.Make("Something went wrong trying to create the family.").Show();
            }
        }

        [RelayCommand]
        public async Task GoToAddAnimal()
        {
            if (RegisterModel == null)
            {
                var navItems = new Dictionary<string, object>
                {
                    {"sendBack", true }
                };
                await Shell.Current.GoToAsync("AddAnimalPageStacked", navItems);
            }
        }
    }
}
