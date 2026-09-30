using CelebriStaff.Services.Api;
using ZXing.Net.Maui;

namespace CelebriStaff.Views;

/// <summary>Lê o QR do convite e abre a tela de entrada com os dados dele.</summary>
public partial class LeitorQrPage : ContentPage, IQueryAttributable
{
    private readonly ApiClient _api;
    private string _eventoId = string.Empty;
    private bool _processando;

    public LeitorQrPage(ApiClient api)
    {
        InitializeComponent();
        _api = api;
        Leitor.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false,
        };
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("eventoId", out var id)) _eventoId = id?.ToString() ?? string.Empty;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _processando = false;
        Leitor.IsDetecting = true;
    }

    protected override void OnDisappearing()
    {
        Leitor.IsDetecting = false;
        base.OnDisappearing();
    }

    private void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        var valor = e.Results.FirstOrDefault()?.Value;
        if (string.IsNullOrWhiteSpace(valor) || _processando) return;
        _processando = true;

        MainThread.BeginInvokeOnMainThread(async () => await ProcessarAsync(valor));
    }

    private async Task ProcessarAsync(string token)
    {
        Leitor.IsDetecting = false;
        Carregando.IsRunning = Carregando.IsVisible = true;
        try
        {
            var result = await _api.LerQrCodeAsync(_eventoId, token);
            if (result is { Success: true, Data: not null })
            {
                // Troca o leitor pela tela de entrada: voltar dela leva à portaria.
                await Shell.Current.GoToAsync("../portaria-entrada", new Dictionary<string, object>
                {
                    ["eventoId"] = _eventoId,
                    ["convite"] = result.Data,
                });
                return;
            }

            Mensagem.Text = $"{result.Message ?? "Convite não reconhecido."} Aponte para outro QR code.";
            await Task.Delay(1500);
            _processando = false;
            Leitor.IsDetecting = true;
        }
        finally
        {
            Carregando.IsRunning = Carregando.IsVisible = false;
        }
    }
}
