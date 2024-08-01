using AniFoodNew.ViewModels.AddViewModels;

namespace AniFoodNew.Views.AddPages;
[QueryProperty(nameof(TempAnimal), "animalModel")]
public partial class CreateFamilyPage : ContentPage
{
    private AnimalRegisterModel? _tempAnimal;

    public AnimalRegisterModel? TempAnimal
    {
        get { return _tempAnimal; }
        set
        {
            _tempAnimal = value;
            if (BindingContext is CreateFamilyViewModel model)
                model.RegisterModel = value;
        }
    }
    public CreateFamilyPage(CreateFamilyViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}