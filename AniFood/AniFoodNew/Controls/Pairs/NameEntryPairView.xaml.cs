namespace AniFoodNew.Controls;

[Obsolete("Could not figure out how to do this the MVVM way")]
public partial class NameEntryPairView : ContentView
{
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(NameEntryPairView));

    public static readonly BindableProperty EntryValueProperty =
        BindableProperty.Create(nameof(EntryValue), typeof(string), typeof(NameEntryPairView));

    public string Name
    {
        get { return (string)GetValue(NameProperty); }
        set { SetValue(NameProperty, value); }
    }

    public string EntryValue
    {
        get { return (string)GetValue(EntryValueProperty); }
        set { SetValue(EntryValueProperty, value); }
    }

    public NameEntryPairView()
    {
        InitializeComponent();
    }
}
