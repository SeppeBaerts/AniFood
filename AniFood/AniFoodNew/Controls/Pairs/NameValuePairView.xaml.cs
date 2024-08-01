using AniFoodNew.ViewModels.ControlViewModels;

namespace AniFoodNew.Controls;


public partial class NameValuePairView : ContentView
{
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(NameValuePairView), default(string));

    public string Name
    {
        get { return (string)GetValue(NameProperty); }
        set { SetValue(NameProperty, value); }
    }

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(string), typeof(NameValuePairView), default(string));

    public string Value
    {
        get { return (string)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public NameValuePairView()
    {
        InitializeComponent();
    }
}
