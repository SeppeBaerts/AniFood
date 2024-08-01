using Microsoft.Maui.Controls.Handlers.Items;

namespace AniFoodNew
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            BaseViewModel.SetTheme(BaseViewModel.GetCurrentTheme(), true);
            MainPage = new AppShell();
        }
    }
}
