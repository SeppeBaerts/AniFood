namespace AniFoodNew.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel login)
	{
		InitializeComponent();
		BindingContext = login;
	}
}