using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.ViewModels.PopupViewModels
{
    public partial class MainPopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _title;
        [ObservableProperty]
        private string _description;
        [ObservableProperty]
        private bool _closeOnOutsideClick;
        [ObservableProperty]
        private string _buttonRight;
        [ObservableProperty]
        private string? _buttonLeft;
        [ObservableProperty]
        private RelayCommand _rightButtonClickedCommand;
        [ObservableProperty]
        private RelayCommand? _leftButtonClickedCommand;

        [ObservableProperty]
        private bool _hasLeftButtonProperties;

        public MainPopupViewModel(string title, string description, bool closeOnOutsideClick, string buttonRight, RelayCommand? rightButtonClickedCommand, string? buttonLeft,  RelayCommand? leftButtonClickedCommand)
        {
            Title = title;
            Description = description;
            CloseOnOutsideClick = closeOnOutsideClick;
            ButtonRight = buttonRight;
            ButtonLeft = buttonLeft;
            RightButtonClickedCommand = rightButtonClickedCommand?? emptyRelayCommandCommand!;
            LeftButtonClickedCommand = leftButtonClickedCommand;
            HasLeftButtonProperties = buttonLeft != null && leftButtonClickedCommand != null;
        }
        [RelayCommand]
        public void EmptyRelayCommand()
        {

        }
    }
}
