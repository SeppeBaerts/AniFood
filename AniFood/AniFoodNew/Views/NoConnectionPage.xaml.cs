namespace AniFoodNew.Views;

public partial class NoConnectionPage : ContentPage
{
	public NoConnectionPage(NoConnectionViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}