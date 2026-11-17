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
#if WINDOWS
        AjustarJanelaTamanhoCelular(window);
#endif
        return window;
    }

#if WINDOWS
    /// <summary>
    /// No Windows a janela abre no tamanho de um celular (≈ Galaxy A55, em dp),
    /// centralizada, para testar a UI no PC com a mesma proporção do aparelho.
    /// Continua redimensionável; a altura é limitada à tela em monitores baixos.
    /// </summary>
    private static void AjustarJanelaTamanhoCelular(Window window)
    {
        const double largura = 412;
        const double altura = 892;
        const double folgaVertical = 80; // barra de tarefas + barra de título

        var tela = DeviceDisplay.MainDisplayInfo;
        var larguraTela = tela.Width / tela.Density;
        var alturaTela = tela.Height / tela.Density;

        window.MinimumWidth = 360;
        window.Width = largura;
        window.Height = Math.Min(altura, alturaTela - folgaVertical);
        window.X = Math.Max(0, (larguraTela - window.Width) / 2);
        window.Y = Math.Max(0, (alturaTela - window.Height - folgaVertical) / 2);
    }
#endif
}
