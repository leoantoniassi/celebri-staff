namespace CelebriStaff.Views.Behaviors;

/// <summary>Anima o conteúdo da página sempre que ela aparece (fade + slide).</summary>
public static class PageEntrance
{
    public static readonly BindableProperty EnabledProperty = BindableProperty.CreateAttached(
        "Enabled", typeof(bool), typeof(PageEntrance), false,
        propertyChanged: OnEnabledChanged);

    public static bool GetEnabled(BindableObject view) => (bool)view.GetValue(EnabledProperty);

    public static void SetEnabled(BindableObject view, bool value) => view.SetValue(EnabledProperty, value);

    private static void OnEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Page page) return;

        page.Appearing -= OnPageAppearing;

        if ((bool)newValue)
            page.Appearing += OnPageAppearing;
    }

    private static void OnPageAppearing(object? sender, EventArgs e)
    {
        if (sender is ContentPage page)
            Motion.PlayEntrance(page.Content);
    }
}
