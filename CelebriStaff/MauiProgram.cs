using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Behaviors;
using CelebriStaff.Services.Api;
using CelebriStaff.Services.Auth;
using CelebriStaff.Services.Feedback;
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
        // O campo já é desenhado pelo Border (InputBorderStyle): tira o sublinhado nativo do Android.
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("SemSublinhado", (handler, _) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        // Feedback de toque em todos os botões, sem repetir Behavior em cada tela.
        Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("FeedbackToque", (_, view) =>
        {
            if (view is not Button button || button.Behaviors.OfType<TouchBehavior>().Any()) return;

            button.Behaviors.Add(new TouchBehavior
            {
                PressedScale = 0.97,
                PressedOpacity = 0.86,
                PressedAnimationDuration = 90,
                HoveredScale = 1.01,
                HoveredAnimationDuration = 120,
            });
        });

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("PlusJakartaSans-Regular.ttf", "PlusJakartaSans");
                fonts.AddFont("PlusJakartaSans-SemiBold.ttf", "PlusJakartaSansSemibold");
                fonts.AddFont("PlusJakartaSans-Bold.ttf", "PlusJakartaSansBold");
                fonts.AddFont("Inter-Regular.ttf", "Inter");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
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
        builder.Services.AddSingleton<IFeedbackService, FeedbackService>();

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
