using AniFoodNew.Models.Classes;
using AniFoodNew.ViewModels.ChangeViewModels;

namespace AniFoodNew.Views.Change;

[QueryProperty(nameof(ChangingAnimal), "animal")]
public partial class ChangeAnimalPage : ContentPage
{
    Animal changingAnimal;
    public Animal ChangingAnimal
    {
        get => changingAnimal;
        set
        {
            changingAnimal = value;
            if (BindingContext is ChangeAnimalViewModel ani)
                ani.Animal = changingAnimal;
        }
    }
    public ChangeAnimalPage(ChangeAnimalViewModel model)
	{
		InitializeComponent();
        model.Animal = ChangingAnimal;
		BindingContext = model;
	}
}