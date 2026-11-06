namespace CelebriStaff.Views.Behaviors;

/// <summary>Microanimações compartilhadas pelas telas.</summary>
public static class Motion
{
    public static void PlayEntrance(VisualElement? content, uint duration = 300)
    {
        if (content is null) return;

        content.Opacity = 0;
        content.TranslationY = 14;

        _ = content.FadeToAsync(1, duration, Easing.CubicOut);
        _ = content.TranslateToAsync(0, 0, duration, Easing.CubicOut);
    }
}
