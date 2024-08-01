namespace AniFoodNew.Views;

public partial class AddFoodPage : ContentPage
{
	public AddFoodPage(AddFoodViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}