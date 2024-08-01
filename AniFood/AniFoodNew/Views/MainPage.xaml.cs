namespace AniFoodNew.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel main)
    {
        InitializeComponent();
        BindingContext = main;
    }
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        (BindingContext as MainViewModel)?.SetButtons(AllButton, FavButton);
        (BindingContext as MainViewModel)?.Init();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MainViewModel)?.OnAppearing();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as MainViewModel)?.OnDissapearing();
    }
}