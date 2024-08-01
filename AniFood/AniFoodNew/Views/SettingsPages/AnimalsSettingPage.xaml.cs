using AniFoodNew.ViewModels.SettingsViewModels;

namespace AniFoodNew.Views.SettingsPages;

public partial class AnimalsSettingPage : ContentPage
{
	public AnimalsSettingPage(AnimalsSettingsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}