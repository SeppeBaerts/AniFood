using AniFoodNew.ViewModels.PopupViewModels;
using CommunityToolkit.Maui.Views;

namespace AniFoodNew.Controls.Popups;

public partial class MainPopupPage : Popup
{
    /// <summary>
    /// Generates a Generic popup, both buttons will close the popup
    /// </summary>
    /// <param name="title">The title of the popup</param>
    /// <param name="description">The description of the popup</param>
    /// <param name="closeOnOutsideClick">Defines if the user can click outside of the popup to dismiss it</param>
    /// <param name="buttonRight">The text showed in the right button</param>
    /// <param name="rightButtonClickedCommand">The command to execute when the right button is pressed (this will not override the close)</param>
    /// <param name="buttonLeft">The text showed in the left button</param>
    /// <param name="leftButtonClickedCommand">the command to execute when the left button is pressed. (this will not override the close)</param>
    public MainPopupPage(string title, string description, bool closeOnOutsideClick,string buttonRight, RelayCommand? rightButtonClickedCommand, string? buttonLeft= null, RelayCommand? leftButtonClickedCommand = null)
	{
		InitializeComponent();
		MainPopupViewModel model = new(title, description, closeOnOutsideClick, buttonRight, rightButtonClickedCommand, buttonLeft, leftButtonClickedCommand);
		BindingContext = model;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Close();
    }
}