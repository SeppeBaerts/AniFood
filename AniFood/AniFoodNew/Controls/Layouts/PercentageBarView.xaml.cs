namespace AniFoodNew.Controls.Layouts;
[Obsolete("Could not figure out how to do this the MVVM way")]
public partial class PercentageBar : ContentView, INotifyPropertyChanged
{
    #region Bindable Properties
    #region Declaring Bindables
    public static readonly BindableProperty PercentageBarNameProperty =
        BindableProperty.Create(nameof(PercentageBarName), typeof(string), typeof(PercentageBar));
    public static readonly BindableProperty MaxValueProperty =
        BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(PercentageBar), propertyChanged: MaxOrCurrentValueChanged);
    public static readonly BindableProperty CurrentValueProperty =
        BindableProperty.Create(nameof(CurrentValue), typeof(int), typeof(PercentageBar), propertyChanged: MaxOrCurrentValueChanged);
    public static readonly BindableProperty ShowPercentageProperty =
        BindableProperty.Create(nameof(ShowPercentage), typeof(bool), typeof(PercentageBar), true);
    public static readonly BindableProperty PercentageBarValuesProperty =
       BindableProperty.Create(nameof(PercentageBarValues), typeof(int[]), typeof(PercentageBar), propertyChanged: PercentageBarValuesChanged);
    #endregion
    public string PercentageBarName
    {
        get { return (string)GetValue(PercentageBarNameProperty); }
        set { SetValue(PercentageBarNameProperty, value); }
    }
    public int[] PercentageBarValues
    {
        get { return (int[])GetValue(PercentageBarValuesProperty); }
        set { SetValue(PercentageBarValuesProperty, value); }
    }
    public int CurrentValue
    {
        get { return (int)GetValue(CurrentValueProperty); }
        set { SetValue(CurrentValueProperty, value); }
    }
    public int MaxValue
    {
        get { return (int)GetValue(MaxValueProperty); }
        set { SetValue(MaxValueProperty, value); }
    }
    public bool ShowPercentage
    {
        get { return (bool)GetValue(ShowPercentageProperty); }
        set { SetValue(ShowPercentageProperty, value); }
    }
    public string? Percentage { get; set; }
    #endregion
    public PercentageBar()
    {
        InitializeComponent();
    }
    static void MaxOrCurrentValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PercentageBar)bindable;
        control.Percentage = $"{control.CurrentValue * 100 / control.MaxValue}%";
    }
    static void PercentageBarValuesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PercentageBar)bindable;
        if (newValue is not int[] values)
            return;
        control.Percentage = $"{values[0] * 100 / (values[0] + values[1])}%";
        control.OnPropertyChanged(nameof(PercentageBarValues));
    }
}
