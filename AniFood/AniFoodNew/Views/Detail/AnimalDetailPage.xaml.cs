using AniFoodNew.Models.Classes;
using AniFoodNew.ViewModels.Detail;

namespace AniFoodNew.Views.Detail;
[QueryProperty(nameof(DetailAnimal), "animal")]
public partial class AnimalDetailPage : ContentPage
{
    Animal detailAnimal;
    public Animal DetailAnimal
    {
        get => detailAnimal;
        set
        {
            detailAnimal = value;
            if(BindingContext is AnimalDetailViewModel ani)
                ani.Animal = detailAnimal;
        }
    }
    public AnimalDetailPage(AnimalDetailViewModel model)
	{
		InitializeComponent();
        model.Animal = DetailAnimal;
		BindingContext = model;
	}
}