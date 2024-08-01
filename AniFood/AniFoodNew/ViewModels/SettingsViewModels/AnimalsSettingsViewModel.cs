using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels.SettingsViewModels
{
    public partial class AnimalsSettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool _feedOnSwipe = Preferences.Default.Get(SwipeToFeedLocation, true);
        partial void OnFeedOnSwipeChanged(bool value)
        {
            Preferences.Default.Set(SwipeToFeedLocation, value);
        }
    }
}
