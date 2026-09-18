using CommunityToolkit.Maui;
using CelebriStaff.Services.Api;
using CelebriStaff.Services.Auth;
using CelebriStaff.Services.Theme;
using CelebriStaff.ViewModels;
using CelebriStaff.Views;
using Microsoft.Extensions.Logging;

namespace CelebriStaff;

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
