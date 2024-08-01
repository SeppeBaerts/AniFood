using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels.ControlViewModels
{
    [Obsolete("This class cannot be used because controls cannot have viewmodels.")]
    public partial class NameValuePairViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _value;
    }
}
