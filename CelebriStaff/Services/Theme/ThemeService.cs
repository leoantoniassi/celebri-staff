using System.ComponentModel;
using CelebriStaff.Models;

namespace CelebriStaff.Services.Theme;

/// <summary>
/// Aplica a identidade visual (cores + logo) do buffet contratante e o tema
/// claro/escuro em tempo de execução. Tudo vira Application.Current.Resources,
/// referenciado nas telas via {DynamicResource ...} — atualiza a UI
/// imediatamente, sem reiniciar o app. O LogoUrl fica exposto por
/// INotifyPropertyChanged para telas com {Binding} num Image.
/// </summary>
public class ThemeService : INotifyPropertyChanged
{
    private const double MinContrast = 4.5;

    private static readonly Color OnDark = Color.FromArgb("#021A1D");

    private string? _logoUrl;
    private string _nomeFantasia = "Celebri Staff";

    // Paleta padrão do Celebri (igual ao painel web).
    private Color _primary = Color.FromArgb("#1CEAFF");
    private Color _secondary = Color.FromArgb("#FF45FF");
    private Color _tertiary = Color.FromArgb("#1F357F");
    private bool _dark;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? LogoUrl
    {
        get => _logoUrl;
        private set
        {
            _logoUrl = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogoUrl)));
        }
    }

    public string NomeFantasia
    {
        get => _nomeFantasia;
        private set
        {
            _nomeFantasia = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomeFantasia)));
        }
    }

    /// <summary>Cores do buffet (white-label).</summary>
    public void Apply(TenantConfig tenant)
    {
        _primary = Parse(tenant.Cores.Primaria, _primary);
        _secondary = Parse(tenant.Cores.Secundaria, _secondary);
        _tertiary = Parse(tenant.Cores.Terciaria, _tertiary);

        LogoUrl = tenant.LogoUrl;
        NomeFantasia = tenant.NomeFantasia;

        ApplyBrand();
    }

    /// <summary>Tema claro/escuro do sistema.</summary>
    public void ApplyMode(AppTheme theme)
    {
        var resources = Application.Current?.Resources;
        if (resources is null) return;

        _dark = theme == AppTheme.Dark;

        if (_dark)
        {
            Set(resources, "ColorBackground", "#111417");
            Set(resources, "ColorSurface", "#181D22");
            Set(resources, "ColorSurfaceAlt", "#1E2429");
            Set(resources, "ColorTextPrimary", "#EDF2F5");
            Set(resources, "ColorTextSecondary", "#B6C4CF");
            Set(resources, "ColorBorder", "#2E3842");
            Set(resources, "ColorShadow", "#66000000");
            Set(resources, "ColorSuccessContainer", "#1B5E20");
            Set(resources, "ColorWarningContainer", "#7E2A00");
            Set(resources, "ColorInfoContainer", "#0D47A1");
            Set(resources, "ColorErrorContainer", "#93000A");
            Set(resources, "ColorOnSuccessContainer", "#D7F0D5");
            Set(resources, "ColorOnWarningContainer", "#FFE0B2");
            Set(resources, "ColorOnInfoContainer", "#D6E6FF");
            Set(resources, "ColorOnErrorContainer", "#FFDAD6");
        }
        else
        {
            Set(resources, "ColorBackground", "#FAFCFD");
            Set(resources, "ColorSurface", "#FFFFFF");
            Set(resources, "ColorSurfaceAlt", "#F3F6F6");
            Set(resources, "ColorTextPrimary", "#020708");
            Set(resources, "ColorTextSecondary", "#566E71");
            Set(resources, "ColorBorder", "#B0CACD");
            Set(resources, "ColorShadow", "#14000000");
            Set(resources, "ColorSuccessContainer", "#D7F0D5");
            Set(resources, "ColorWarningContainer", "#FFE0B2");
            Set(resources, "ColorInfoContainer", "#D6E6FF");
            Set(resources, "ColorErrorContainer", "#FFDAD6");
            Set(resources, "ColorOnSuccessContainer", "#0B2E0D");
            Set(resources, "ColorOnWarningContainer", "#3B1A00");
            Set(resources, "ColorOnInfoContainer", "#001A3D");
            Set(resources, "ColorOnErrorContainer", "#410002");
        }

        ApplyBrand();
    }

    private void ApplyBrand()
    {
        var resources = Application.Current?.Resources;
        if (resources is null) return;

        resources["ColorPrimary"] = _primary;
        resources["ColorSecondary"] = _secondary;
        resources["ColorTertiary"] = _tertiary;
        resources["ColorOnPrimary"] = BestOn(_primary);
        resources["ColorOnSecondary"] = BestOn(_secondary);

        // Cor de marca segura para texto/ícones: azul-marinho no claro, ciano no escuro,
        // ajustada até atingir contraste AA sobre o fundo do tema.
        var surface = _dark ? Color.FromArgb("#181D22") : Colors.White;
        resources["ColorBrandText"] = EnsureContrast(_dark ? _primary : _tertiary, surface);
    }

    // ── Contraste (WCAG) ──────────────────────────────────────────

    private static Color Parse(string? hex, Color fallback)
    {
        if (string.IsNullOrWhiteSpace(hex)) return fallback;
        return Color.TryParse(hex, out var c) ? c : fallback;
    }

    private static void Set(ResourceDictionary r, string key, string hex) => r[key] = Color.FromArgb(hex);

    private static Color BestOn(Color background)
        => Contrast(OnDark, background) >= Contrast(Colors.White, background) ? OnDark : Colors.White;

    private static Color EnsureContrast(Color color, Color against)
    {
        var target = Luminance(against) < 0.5 ? Colors.White : Colors.Black;
        var current = color;
        for (var i = 0; i < 20 && Contrast(current, against) < MinContrast; i++)
            current = Mix(current, target, 0.1f);
        return current;
    }

    private static Color Mix(Color a, Color b, float t)
        => new(a.Red + (b.Red - a.Red) * t, a.Green + (b.Green - a.Green) * t, a.Blue + (b.Blue - a.Blue) * t);

    private static double Luminance(Color c)
    {
        static double Lin(double v) => v <= 0.03928 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
        return 0.2126 * Lin(c.Red) + 0.7152 * Lin(c.Green) + 0.0722 * Lin(c.Blue);
    }

    private static double Contrast(Color a, Color b)
    {
        var (hi, lo) = (Luminance(a), Luminance(b));
        if (hi < lo) (hi, lo) = (lo, hi);
        return (hi + 0.05) / (lo + 0.05);
    }
}
