using AniFoodNew.DataB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class LoadingPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private double _progress;
        public ProgressBar progressBar { get; set; }

        public async Task Init()
        {
            var answer = await ServerGetter.HasAuthorisation();
            if (answer.Errors.Count == 0)
            {
                if (answer.ServerResponse)
                {
                    
                    Progress<double> pro = new Progress<double>(ReportProgress);
                    await ServerGetter.FillUserAsync(pro);
                    await Shell.Current.GoToAsync(
                        MainUser!.Families.Any(f => f.Animals.Count == 0) ? 
                        "///AddAnimalPage" : "///MainPage");
                }
                else
                    await Shell.Current.GoToAsync("///LoginPage");
            }
            else
                await Shell.Current.GoToAsync("///NoConnectionPage");
        }
        private void ReportProgress(double progress)
        {
            ChangeBarAsync(progress);
        }
        private async Task ChangeBarAsync(double progress)
        {
            await progressBar.ProgressTo(progress, 50, Easing.SinOut);
        }
    }
}
