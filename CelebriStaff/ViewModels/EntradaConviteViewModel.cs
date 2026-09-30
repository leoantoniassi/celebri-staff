using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Models;
using CelebriStaff.Services.Api;

namespace CelebriStaff.ViewModels;

/// <summary>Registra quantas pessoas de um convite entraram (a portaria pode ajustar o número).</summary>
public partial class EntradaConviteViewModel(ApiClient api) : ObservableObject, IQueryAttributable
{
    private string _eventoId = string.Empty;
    private string _conviteId = string.Empty;

    [ObservableProperty]
    private string nome = string.Empty;

    [ObservableProperty]
    private string situacao = string.Empty;

    [ObservableProperty]
    private int quantidade = 1;

    [ObservableProperty]
    private int jaEntraram;

    [ObservableProperty]
    private string? mensagem;

    [ObservableProperty]
    private Color mensagemCor = Colors.Gray;

    [ObservableProperty]
    private bool isBusy;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("eventoId", out var id)) _eventoId = id?.ToString() ?? string.Empty;
        if (query.TryGetValue("convite", out var obj) && obj is ConvitePortaria convite)
        {
            _conviteId = convite.Id;
            Nome = convite.Nome;
            Aplicar(convite);
            // Sugere quem ainda falta entrar; a portaria pode mudar antes de confirmar.
            Quantidade = Math.Max(convite.QtdPessoas - convite.QtdEntrou, 1);
        }
    }

    private void Aplicar(ConvitePortaria convite)
    {
        JaEntraram = convite.QtdEntrou;
        var faltam = convite.QtdPessoas - convite.QtdEntrou;
        Situacao = faltam > 0
            ? $"Convite para {convite.QtdPessoas} · já entraram {convite.QtdEntrou} · faltam {faltam}"
            : $"Convite para {convite.QtdPessoas} · já entraram {convite.QtdEntrou} (completo)";
    }

    [RelayCommand]
    private void Mais() => Quantidade = Math.Min(Quantidade + 1, 100);

    [RelayCommand]
    private void Menos() => Quantidade = Math.Max(Quantidade - 1, 1);

    [RelayCommand]
    private Task RegistrarAsync() => EnviarAsync(Quantidade, voltar: true);

    [RelayCommand]
    private Task DesfazerUmaAsync() => EnviarAsync(-1, voltar: false);

    private async Task EnviarAsync(int quantidade, bool voltar)
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var result = await api.RegistrarEntradaAsync(_eventoId, _conviteId, quantidade);
            if (result is not { Success: true, Data: not null })
            {
                Mensagem = result.Message ?? "Não foi possível registrar.";
                MensagemCor = Colors.Red;
                return;
            }

            Aplicar(result.Data);
            Mensagem = result.Message;
            MensagemCor = result.Data.Excedente > 0 ? Color.FromArgb("#B26A00") : Color.FromArgb("#1B7F3B");

            if (voltar && result.Data.Excedente == 0)
            {
                await Task.Delay(700);
                await Shell.Current.GoToAsync("..");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
