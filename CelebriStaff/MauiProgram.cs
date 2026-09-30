using CommunityToolkit.Maui;
using CelebriStaff.Services.Api;
using CelebriStaff.Services.Auth;
using CelebriStaff.Services.Theme;
using CelebriStaff.ViewModels;
using CelebriStaff.Views;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace CelebriStaff;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // ── Auth / rede ───────────────────────────────────────────
        builder.Services.AddSingleton<ITokenStore, SecureTokenStore>();
        builder.Services.AddTransient<AuthHeaderHandler>();

        builder.Services
            .AddHttpClient<ApiClient>(client =>
            {
                client.BaseAddress = new Uri(AppConfig.ApiBaseUrl);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();

        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<ThemeService>();

        // ── Shell / páginas ──────────────────────────────────────
        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddTransient<EmailViewModel>();
        builder.Services.AddTransient<EmailPage>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<PrimeiroAcessoViewModel>();
        builder.Services.AddTransient<PrimeiroAcessoPage>();

        builder.Services.AddTransient<PortariaViewModel>();
        builder.Services.AddTransient<PortariaPage>();
        builder.Services.AddTransient<LeitorQrPage>();
        builder.Services.AddTransient<EntradaConviteViewModel>();
        builder.Services.AddTransient<EntradaConvitePage>();

        builder.Services.AddTransient<GarcomViewModel>();
        builder.Services.AddTransient<GarcomPage>();
        builder.Services.AddTransient<MesaViewModel>();
        builder.Services.AddTransient<MesaPage>();
        builder.Services.AddTransient<CozinhaViewModel>();
        builder.Services.AddTransient<CozinhaPage>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomePage>();

        return builder.Build();
    }
}
