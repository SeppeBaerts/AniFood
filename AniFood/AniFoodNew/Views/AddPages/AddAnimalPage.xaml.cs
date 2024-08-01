namespace AniFoodNew.Views;
[QueryProperty(nameof(SendBack), "sendBack")]
public partial class AddAnimalPage : ContentPage
{
    bool _sentBack;
    public bool SendBack
    {
        get => _sentBack;
        set
        {
            _sentBack = value;
            if (BindingContext is AddAnimalViewModel model)
                model.SentAnimalInfoBack = value;
        }
    }
    public AddAnimalPage(AddAnimalViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}