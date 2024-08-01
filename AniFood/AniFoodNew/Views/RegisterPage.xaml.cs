namespace AniFoodNew.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel register)
	{
		InitializeComponent();
		BindingContext = register;
	}
}