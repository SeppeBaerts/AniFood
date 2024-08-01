namespace AniFoodNew.Controls.Cards;

[Obsolete("Could not figure out how to do this the MVVM way")]
public partial class UserCardView : ContentView
{
    public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(UserCardView), "dotnet_bot.png");
    public string ImageUrl
    {
        get { return (string)GetValue(ImageUrlProperty); }
        set { SetValue(ImageUrlProperty, value); }
    }

    public static readonly BindableProperty UserNameProperty = BindableProperty.Create(nameof(UserName), typeof(string), typeof(UserCardView));
    public string UserName
    {
        get { return (string)GetValue(UserNameProperty); }
        set { SetValue(UserNameProperty, value); }
    }

    public static readonly BindableProperty RoleProperty = BindableProperty.Create(nameof(Role), typeof(string), typeof(UserCardView), "Member");
    public string Role
    {
        get { return (string)GetValue(RoleProperty); }
        set { SetValue(RoleProperty, value); }
    }
    public UserCardView()
	{
		InitializeComponent();
	}
}