using AniFoodNew.ViewModels.SettingsViewModels;

namespace AniFoodNew.Views.SettingsPages;

public partial class FamilySettingsPage : ContentPage
{
	public FamilySettingsPage(FamilySettingsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}