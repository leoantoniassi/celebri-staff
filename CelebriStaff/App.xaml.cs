using CelebriStaff.Services.Theme;

namespace CelebriStaff;

public partial class App : Application
{
    private readonly AppShell _shell;

    public App(AppShell shell, ThemeService theme)
    {
        InitializeComponent();
        _shell = shell;

        // Tema claro/escuro segue o sistema; ThemeService troca os tokens em runtime.
        theme.ApplyMode(RequestedTheme);
        RequestedThemeChanged += (_, e) => theme.ApplyMode(e.RequestedTheme);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_shell);
    }
}
