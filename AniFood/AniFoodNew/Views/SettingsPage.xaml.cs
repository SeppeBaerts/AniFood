namespace AniFoodNew.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}