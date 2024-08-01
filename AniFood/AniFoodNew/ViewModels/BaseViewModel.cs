using AniFoodNew.DataB;
using AniFoodNew.Models.Classes;
using AniFoodNew.Resources.Styles;
using AniFoodNew.Resources.Styles.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Devices.Sensors;

namespace AniFoodNew.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    #region LocationStrings
    private const string ThemeLocation = "ThemeKey";
    private const string FavoriteLocation = "Favkey";
    internal const string SendFoodNotificationLocation = "SendFoodNotificationKey";
    internal const string SendFeedNotificationLocation = "SendFeedNotificationKey";
    internal const string ShowFoodLocation = "showFoodKey";
    internal const string AutoSwitchFamLocation = "autoSwitchFamKey";
    internal const string SwipeToFeedLocation = "swipeToFeedKey";
    internal const string HighestNotificationIdLocation = "highestNotificationIdKey";
    internal const string MainAppThemeLocation = "mainAppThemeKey";
    #endregion

    #region properties and variables
    internal protected static FullUser? MainUser { get; set; }

    #region Lists
    internal static List<Guid> Favorites = [];
    internal static List<FullFamily> AllFamilies => MainUser.Families;
    internal static List<Animal> AllAnimals => AllFamilies.SelectMany(f => f.Animals).ToList();
    internal static List<Food> AllFoods => AllFamilies.SelectMany(f => f.Foods).ToList();
    #endregion

    #endregion
    #region Connectors
    internal protected static ServerGetConnector ServerGetter { get; set; } = new();
    internal protected static ServerSendConnector ServerSender { get; set; } = new();
    #endregion



    /// <summary>
    /// Gets The Theme currently in preferences.
    /// </summary>
    /// <returns>enum of Theme</returns>
    internal static Theme GetCurrentTheme()
    {
        if (!Preferences.Default.ContainsKey(ThemeLocation))
            return Theme.Default;

        var enumNumber = Preferences.Default.Get(ThemeLocation, 0);
        return (Theme)enumNumber;
    }
    internal static MainTheme GetCurrentMainTheme()
    {
        if(!Preferences.Default.ContainsKey(MainAppThemeLocation))
            return MainTheme.System;

        var enumNumber = Preferences.Default.Get(MainAppThemeLocation, 0);
        return (MainTheme)enumNumber;
    }
    private static ResourceDictionary GetTheme(Theme theme)
    {
        return theme switch
        {
            Theme.Default => new DefaultTheme(),
            Theme.Blue => new BlueTheme(),
            Theme.Red => new RedTheme(),
            Theme.HighContrast => new HighContrastTheme(),
            _ => new DefaultTheme()
        };
    }
    private static ResourceDictionary GetTheme(MainTheme theme)
    {
        //Ik weet dat de system default vaak light is, maar Dark is gewoonweg veel mooier in deze app.
        return theme switch
        {
            MainTheme.System => Application.Current.PlatformAppTheme switch
            {
                AppTheme.Dark => new DarkTheme(),
                AppTheme.Light => new LightTheme(),
                _ => new DarkTheme()
            },
            MainTheme.Dark => new DarkTheme(),
            MainTheme.Light => new LightTheme(),
            _ => new DarkTheme()
        };

    }
    public static void SetTheme(Theme theme, bool isStartup = false)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(GetTheme(GetCurrentMainTheme()));
            mergedDictionaries.Add(GetTheme(theme));
            AddDefaultThemes(mergedDictionaries);
            if (!isStartup)
                Preferences.Default.Set(ThemeLocation, (int)theme);
        }
    }
    public static void SetMainTheme(MainTheme theme, bool isStartup = false)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(GetTheme(theme));
            mergedDictionaries.Add(GetTheme(GetCurrentTheme()));
            AddDefaultThemes(mergedDictionaries);
            if (!isStartup)
                Preferences.Default.Set(MainAppThemeLocation, (int)theme);
        }
    }
    private static void AddDefaultThemes(ICollection<ResourceDictionary> dic)
    {
        dic.Add(new DefaultColors());
        dic.Add(new DefaultStyles());
    }

    internal async static Task<Location?> GetCurrentLocation(IGeolocation loc)
    {
        var location = await loc.GetLocationAsync(
        new GeolocationRequest
        {
            DesiredAccuracy = GeolocationAccuracy.High,
            Timeout = TimeSpan.FromSeconds(20)
        });
        return location;
    }

    public enum Theme
    {
        Default,
        Blue,
        Red,
        HighContrast
    }
    public enum MainTheme
    {
        System,
        Dark,
        Light
    }
    internal protected static void LogoutUser()
    {
        MainUser = null;
        Preferences.Clear();
        Preferences.Default.Clear();
        ServerGetter.ClearClient();
        ServerSender.ClearClient();
        Application.Current.MainPage = new AppShell();
    }

    #region Favorites
    private static bool hasLoadedFavorites = false;
    public static void LoadFavorites()
    {

        if (!hasLoadedFavorites && Preferences.Default.ContainsKey(FavoriteLocation))
        {
            Favorites = JsonConvert.DeserializeObject<List<Guid>>(Preferences.Default.Get(FavoriteLocation, "")) ?? [];
        }
        hasLoadedFavorites = true;
    }
    public void AddFavorite(Animal ani)
    {
        LoadFavorites();
        if (Favorites.Contains(ani.Id))
            return;
        else
        {
            Favorites.Add(ani.Id);
            ani.IsFavorite = true;
            UpdateFavorites();
        }
    }
    public void RemoveFavorite(Animal ani)
    {
        LoadFavorites();
        if (Favorites.Contains(ani.Id))
        {
            Favorites.Remove(ani.Id);
            ani.IsFavorite = false;
            UpdateFavorites();
        }
    }
    private void UpdateFavorites()
    {
        string guidList = JsonConvert.SerializeObject(Favorites);
        Preferences.Default.Set(FavoriteLocation, guidList);
    }
    #endregion

}
