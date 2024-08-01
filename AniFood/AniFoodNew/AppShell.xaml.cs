using AniFoodNew.Views.AddPages;
using AniFoodNew.Views.Change;
using AniFoodNew.Views.Detail;
using AniFoodNew.Views.Overview;

namespace AniFoodNew
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("AddAnimalPageStacked", typeof(AddAnimalPage));
            Routing.RegisterRoute("AddFoodPageStacked", typeof(AddFoodPage));
            Routing.RegisterRoute(nameof(AnimalDetailPage), typeof(AnimalDetailPage));
            Routing.RegisterRoute(nameof(ChangeAnimalPage), typeof(ChangeAnimalPage));
            Routing.RegisterRoute(nameof(FamilyOverviewPage), typeof(FamilyOverviewPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(CreateFamilyPage), typeof(CreateFamilyPage));
        }
    }
}
