using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CelebriStaff.Services.Feedback;

/// <summary>
/// Feedback transitório para o usuário: toast curto para sucesso/informação e
/// snackbar (mais longo, com "OK") para erro. Erros persistentes de carga
/// continuam no ErrorBanner da tela.
/// </summary>
public interface IFeedbackService
{
    void Success(string message);
    void Info(string message);
    void Error(string message);
}

public class FeedbackService : IFeedbackService
{
    public void Success(string message)
    {
        TryHaptic();
        Toast(message);
    }

    public void Info(string message) => Toast(message);

    public void Error(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var options = new SnackbarOptions
                {
                    BackgroundColor = Application.Current?.Resources.TryGetValue("ColorErrorContainer", out var bg) == true && bg is Color b
                        ? b : Colors.DarkRed,
                    TextColor = Application.Current?.Resources.TryGetValue("ColorOnErrorContainer", out var fg) == true && fg is Color f
                        ? f : Colors.White,
                    ActionButtonTextColor = Application.Current?.Resources.TryGetValue("ColorOnErrorContainer", out var ab) == true && ab is Color a
                        ? a : Colors.White,
                    CornerRadius = new CornerRadius(12),
                };
                await CommunityToolkit.Maui.Alerts.Snackbar
                    .Make(message, null, "OK", TimeSpan.FromSeconds(5), options)
                    .Show();
            }
            catch
            {
                // Sem janela ativa (ex.: app em segundo plano): feedback é só conveniência.
            }
        });
    }

    private static void Toast(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try { await CommunityToolkit.Maui.Alerts.Toast.Make(message, ToastDuration.Short).Show(); }
            catch { /* ver comentário em Error */ }
        });
    }

    private static void TryHaptic()
    {
        try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); }
        catch { /* dispositivo sem vibração */ }
    }
}
