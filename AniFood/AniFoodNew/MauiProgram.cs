using AniFoodNew.Controls;
using AniFoodNew.ViewModels.AddViewModels;
using AniFoodNew.ViewModels.ChangeViewModels;
using AniFoodNew.ViewModels.ControlViewModels;
using AniFoodNew.ViewModels.Detail;
using AniFoodNew.ViewModels.SettingsViewModels;
using AniFoodNew.Views.AddPages;
using AniFoodNew.Views.Change;
using AniFoodNew.Views.Detail;
using AniFoodNew.Views.Overview;
using AniFoodNew.Views.SettingsPages;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace AniFoodNew
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
#if IOS || ANDROID
                .UseLocalNotification()
#endif
                .UseSkiaSharp()
                .UseMauiCommunityToolkitCore()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            #region ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddTransient<AddAnimalViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddTransient<AddFoodViewModel>();
            builder.Services.AddTransient<FamilyOverviewViewModel>();
            builder.Services.AddSingleton<NoConnectionViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddTransient<AnimalDetailViewModel>();
            builder.Services.AddSingleton<ChangeAnimalViewModel>();
            builder.Services.AddSingleton<AnimalsSettingsViewModel>();
            builder.Services.AddSingleton<FamilySettingsViewModel>();
            builder.Services.AddTransient<CreateFamilyViewModel>();
            #endregion

            #region Pages
            builder.Services.AddTransient<AddAnimalPage>();
            builder.Services.AddTransient<AddFoodPage>();
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<FamilyOverviewPage>();
            builder.Services.AddSingleton<NoConnectionPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddTransient<AnimalDetailPage>();
            builder.Services.AddSingleton<ChangeAnimalPage>();
            builder.Services.AddSingleton<AnimalsSettingPage>();
            builder.Services.AddSingleton<FamilySettingsPage>();
            builder.Services.AddTransient<CreateFamilyPage>();
            #endregion
            #region Other
            builder.Services.AddSingleton(Geolocation.Default);
            #endregion
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
