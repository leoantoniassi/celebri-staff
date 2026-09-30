using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Models;
using CelebriStaff.Services.Api;

namespace CelebriStaff.ViewModels;

/// <summary>Portaria: contador de pessoas, leitura de QR e busca de convidados.</summary>
public partial class PortariaViewModel(ApiClient api) : ObservableObject, IQueryAttributable
{
    private string _eventoId = string.Empty;

    [ObservableProperty]
    private string eventoNome = string.Empty;

    [ObservableProperty]
    private PortariaResumo? resumo;

    [ObservableProperty]
    private string totalTexto = "—";

    [ObservableProperty]
    private string previstoTexto = string.Empty;

    [ObservableProperty]
    private bool usaConvites;

    [ObservableProperty]
    private string busca = string.Empty;

    [ObservableProperty]
    private string? erro;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<ConvitePortaria> Convites { get; } = [];

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("eventoId", out var id)) _eventoId = id?.ToString() ?? string.Empty;
        if (query.TryGetValue("eventoNome", out var nome)) EventoNome = nome?.ToString() ?? string.Empty;
    }

    /// <summary>Recarrega os números (chamado ao abrir a tela e a cada poucos segundos).</summary>
    public async Task AtualizarAsync()
    {
        var result = await api.GetPortariaResumoAsync(_eventoId);
        if (!result.Success || result.Data is null)
        {
            Erro = result.Message ?? "Não foi possível carregar a portaria.";
            return;
        }
        Erro = null;
        AplicarResumo(result.Data);
        if (UsaConvites) await BuscarConvitesAsync();
    }

    private void AplicarResumo(PortariaResumo r)
    {
        Resumo = r;
        UsaConvites = r.UsaConvites;
        TotalTexto = r.TotalPresentes.ToString();
        PrevistoTexto = r.QtdPessoasEvento > 0
            ? $"de {r.QtdPessoasEvento} previstos · {r.Avulsos} sem convite"
            : $"{r.Avulsos} sem convite";
    }

    partial void OnBuscaChanged(string value) => _ = BuscarConvitesAsync();

    private async Task BuscarConvitesAsync()
    {
        var result = await api.GetConvitesPortariaAsync(_eventoId, Busca);
        if (!result.Success || result.Data is null) return;

        Convites.Clear();
        foreach (var c in result.Data) Convites.Add(c);
    }

    [RelayCommand]
    private Task EntrouAvulsoAsync() => AjustarAvulsosAsync(1);

    [RelayCommand]
    private Task DesfazerAvulsoAsync() => AjustarAvulsosAsync(-1);

    private async Task AjustarAvulsosAsync(int delta)
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var result = await api.AjustarAvulsosAsync(_eventoId, delta);
            if (result is { Success: true, Data: not null }) AplicarResumo(result.Data);
            else Erro = result.Message ?? "Não foi possível atualizar o contador.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LerQrCodeAsync()
    {
        var permissao = await Permissions.RequestAsync<Permissions.Camera>();
        if (permissao != PermissionStatus.Granted)
        {
            Erro = "Sem permissão de câmera. Libere nas configurações do celular ou busque o convidado pelo nome.";
            return;
        }
        await Shell.Current.GoToAsync("portaria-leitor", new Dictionary<string, object> { ["eventoId"] = _eventoId });
    }

    [RelayCommand]
    private Task AbrirConviteAsync(ConvitePortaria convite)
        => Shell.Current.GoToAsync("portaria-entrada", new Dictionary<string, object>
        {
            ["eventoId"] = _eventoId,
            ["convite"] = convite,
        });
}
