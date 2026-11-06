using CommunityToolkit.Maui.Behaviors;

namespace CelebriStaff.Views.Behaviors;

/// <summary>
/// Feedback de toque global: encolhe levemente e reduz a opacidade ao pressionar.
/// Aplicado via estilo aos botões e por atributo nos cards tocáveis.
/// </summary>
public static class PressFeedback
{
    public static readonly BindableProperty EnabledProperty = BindableProperty.CreateAttached(
        "Enabled", typeof(bool), typeof(PressFeedback), false,
        propertyChanged: OnEnabledChanged);

    public static bool GetEnabled(BindableObject view) => (bool)view.GetValue(EnabledProperty);

    public static void SetEnabled(BindableObject view, bool value) => view.SetValue(EnabledProperty, value);

    private static void OnEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not VisualElement element) return;

        var existing = element.Behaviors.OfType<TouchBehavior>().FirstOrDefault();

        if ((bool)newValue)
        {
            if (existing is not null) return;

            element.Behaviors.Add(new TouchBehavior
            {
                PressedScale = 0.97,
                PressedOpacity = 0.86,
                PressedAnimationDuration = 90,
                HoveredScale = 1.01,
                HoveredAnimationDuration = 120,
            });
        }
        else if (existing is not null)
        {
            element.Behaviors.Remove(existing);
        }
    }
}
