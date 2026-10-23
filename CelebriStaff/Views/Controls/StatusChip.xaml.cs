namespace CelebriStaff.Views.Controls;

public partial class StatusChip : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(StatusChip), string.Empty,
        propertyChanged: (b, _, _) => ((StatusChip)b).Refresh());

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(StatusChip), string.Empty,
        propertyChanged: (b, _, _) => ((StatusChip)b).Refresh());

    public static readonly BindableProperty KindProperty = BindableProperty.Create(
        nameof(Kind), typeof(string), typeof(StatusChip), "neutral",
        propertyChanged: (b, _, _) => ((StatusChip)b).Refresh());

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    /// <summary>Glifo da fonte de ícones (ex.: {StaticResource IconCheckCircle}).</summary>
    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }

    /// <summary>success | warning | error | info | neutral.</summary>
    public string Kind { get => (string)GetValue(KindProperty); set => SetValue(KindProperty, value); }

    public StatusChip()
    {
        InitializeComponent();
        Refresh();
    }

    private void Refresh()
    {
        var (background, foreground) = Kind switch
        {
            "success" => ("ColorSuccessContainer", "ColorOnSuccessContainer"),
            "warning" => ("ColorWarningContainer", "ColorOnWarningContainer"),
            "error" => ("ColorErrorContainer", "ColorOnErrorContainer"),
            "info" => ("ColorInfoContainer", "ColorOnInfoContainer"),
            _ => ("ColorSurfaceAlt", "ColorTextSecondary"),
        };

        // DynamicResource: acompanha a troca de tema claro/escuro em runtime.
        Chip.SetDynamicResource(VisualElement.BackgroundColorProperty, background);
        TextView.SetDynamicResource(Label.TextColorProperty, foreground);
        IconView.SetDynamicResource(Label.TextColorProperty, foreground);

        TextView.Text = Text;
        IconView.Text = Icon;
        IconView.IsVisible = !string.IsNullOrEmpty(Icon);
    }
}
