using CelebriStaff.Services.Theme;

namespace CelebriStaff;

public partial class App : Application
{
    private readonly AppShell _shell;
    private readonly ThemeService _theme;

    public App(AppShell shell, ThemeService theme)
    {
        InitializeComponent();
        _shell = shell;
        _theme = theme;

        // Tema claro/escuro segue o sistema; ThemeService troca os tokens em runtime.
        theme.ApplyMode(RequestedTheme);
        RequestedThemeChanged += (_, e) => theme.ApplyMode(e.RequestedTheme);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(_shell);
        // A Activity só existe aqui: reaplica a barra de status.
        window.Created += (_, _) => _theme.ApplyStatusBar();
        window.Activated += (_, _) => _theme.ApplyStatusBar();
        return window;
    }
}
