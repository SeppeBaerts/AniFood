namespace AniFoodNew.Views.Overview;
public partial class FamilyOverviewPage : ContentPage
{
	public FamilyOverviewPage(FamilyOverviewViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
    protected override void OnAppearing()
    {
		base.OnAppearing();
		(BindingContext as FamilyOverviewViewModel).Refresh();
    }
}