namespace CelebriStaff.Services.Theme;

/// <summary>
/// Acesso, a partir de código (ViewModels), aos tokens de cor do tema atual.
/// Evita hex fixo no C#: as cores vêm de Colors.xaml / ThemeService.
/// </summary>
public static class Tokens
{
    public static Color Get(string key)
        => Application.Current?.Resources.TryGetValue(key, out var v) == true && v is Color c ? c : Colors.Gray;

    public static Color Success => Get("ColorSuccess");
    public static Color Warning => Get("ColorWarning");
    public static Color Error => Get("ColorError");
    public static Color Info => Get("ColorInfo");
    public static Color Border => Get("ColorBorder");
    public static Color TextSecondary => Get("ColorTextSecondary");
}
