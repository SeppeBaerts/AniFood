using CommunityToolkit.Maui.Converters;

namespace AniFoodNew.Controls.Cards;

[Obsolete("Could not figure out how to do this the MVVM way")]
public partial class AnimalCardView : ContentView, INotifyPropertyChanged
{
    #region Bindable Properties Declarations
    public static readonly BindableProperty ImageUriProperty =
        BindableProperty.Create(nameof(ImageUri), typeof(string), typeof(AnimalCardView), "dotnet_bot.png");
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(AnimalCardView));
    public static readonly BindableProperty FoodNameProperty =
        BindableProperty.Create(nameof(FoodName), typeof(string), typeof(AnimalCardView), "unknown");
    public static readonly BindableProperty BreedProperty =
        BindableProperty.Create(nameof(Breed), typeof(string), typeof(AnimalCardView));
    public static readonly BindableProperty BirthdayProperty =
        BindableProperty.Create(nameof(Birthday), typeof(DateTime), typeof(AnimalCardView));
    public static readonly BindableProperty FamilyProperty =
        BindableProperty.Create(nameof(Family), typeof(string), typeof(AnimalCardView));
    public static readonly BindableProperty FoodLeftProperty =
        BindableProperty.Create(nameof(FoodLeft), typeof(int), typeof(AnimalCardView));
    public static readonly BindableProperty FoodGivenProperty =
        BindableProperty.Create(nameof(FoodGiven), typeof(int), typeof(AnimalCardView));
    public static readonly BindableProperty NotesProperty =
        BindableProperty.Create(nameof(Notes), typeof(string), typeof(AnimalCardView), " ");
    public static readonly BindableProperty FoodArrayProperty =
        BindableProperty.Create(nameof(FoodArray), typeof(int[]), typeof(AnimalCardView));
    #endregion
    public int[] FoodArray
    {
        get { return (int[])GetValue(FoodArrayProperty); }
        set { SetValue(FoodArrayProperty, value);
            OnPropertyChanged(nameof(FoodArray));
        }
    }
    public string ImageUri
    {
        get { return (string)GetValue(ImageUriProperty); }
        set { SetValue(ImageUriProperty, value); }
    }
    public string Breed
    {
        get { return (string)GetValue(BreedProperty); }
        set { SetValue(BreedProperty, value); }
    }
    public DateTime Birthday
    {
        get { return (DateTime)GetValue(BirthdayProperty); }
        set { SetValue(BirthdayProperty, value); }
    }
    public string Family
    {
        get { return (string)GetValue(FamilyProperty); }
        set { SetValue(FamilyProperty, value); }
    }
    public int FoodLeft
    {
        get { return (int)GetValue(FoodLeftProperty); }
        set { SetValue(FoodLeftProperty, value); }
    }
    public int FoodGiven
    {
        get { return (int)GetValue(FoodGivenProperty); }
        set
        {
            SetValue(FoodGivenProperty, value);
        }
    }
    public string Notes
    {
        get { return (string)GetValue(NotesProperty); }
        set { SetValue(NotesProperty, value); }
    }
    public string Name
    {
        get { return (string)GetValue(NameProperty); }
        set { SetValue(NameProperty, value); }
    }
    public string AgeInYears
    {
        get
        {
            DateTime today = DateTime.Today;
            int age = today.Year - Birthday.Year;
            if (today < Birthday.AddYears(age))
            {
                age--;
            }
            return age.ToString() + $"Year{(age > 1 ? "s" : "")}";
        }
    }
    public string FoodName
    {
        get { return (string)GetValue(FoodNameProperty); }
        set { SetValue(FoodNameProperty, value); }
    }
    public int MaxFood => FoodGiven + FoodLeft;
    public AnimalCardView()
    {
        InitializeComponent();
    }
}
