using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels
{
    public partial class NoConnectionViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task Refresh()
        {
           await Shell.Current.GoToAsync("///LoadingPage");
        }
    }
}
