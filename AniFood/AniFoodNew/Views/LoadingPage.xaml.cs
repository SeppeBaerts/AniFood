using AniFoodNew.DataB;

namespace AniFoodNew.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingPageViewModel model)
    {
        InitializeComponent();
        model.progressBar = MainProgressBar;
        BindingContext = model;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as LoadingPageViewModel)?.Init();
    }
}