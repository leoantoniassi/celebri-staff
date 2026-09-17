using CommunityToolkit.Maui;
using FestifyStaff.Services.Api;
using FestifyStaff.Services.Auth;
using FestifyStaff.Services.Theme;
using FestifyStaff.ViewModels;
using FestifyStaff.Views;
using Microsoft.Extensions.Logging;

namespace FestifyStaff;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
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

        builder.Services.AddTransient<TenantCodeViewModel>();
        builder.Services.AddTransient<TenantCodePage>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomePage>();

        return builder.Build();
    }
}
