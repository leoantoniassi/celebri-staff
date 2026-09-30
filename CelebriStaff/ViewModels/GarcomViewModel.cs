using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Models;
using CelebriStaff.Services.Api;
using Microsoft.Maui.Layouts;

namespace CelebriStaff.ViewModels;

/// <summary>Mesa desenhada no mapa do salão.</summary>
public class MesaNoMapa(MesaMapa mesa)
{
    private const double Tamanho = 60;

    public MesaMapa Mesa { get; } = mesa;
    public string Numero => Mesa.Numero;

    /// <summary>Posição proporcional (0–1) com tamanho fixo em pixels.</summary>
    public Rect Bounds => new(Mesa.PosX / 100, Mesa.PosY / 100, Tamanho, Tamanho);
    public AbsoluteLayoutFlags Flags => AbsoluteLayoutFlags.PositionProportional;

    public Color Cor => Mesa.Situacao switch
    {
        "pronto" => Color.FromArgb("#1B7F3B"),
        "aguardando" => Color.FromArgb("#E08A00"),
        _ => Color.FromArgb("#9E9E9E"),
    };

    public string Selo => Mesa.PedidosProntos > 0 ? "✓" : Mesa.PedidosAbertos > 0 ? Mesa.PedidosAbertos.ToString() : string.Empty;
}

/// <summary>Garçom: mapa das mesas do salão com a situação dos pedidos.</summary>
public partial class GarcomViewModel(ApiClient api) : ObservableObject, IQueryAttributable
{
    private string _eventoId = string.Empty;

    [ObservableProperty]
    private string eventoNome = string.Empty;

    [ObservableProperty]
    private string? aviso;

    [ObservableProperty]
    private string resumo = string.Empty;

    public ObservableCollection<MesaNoMapa> Mesas { get; } = [];

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("eventoId", out var id)) _eventoId = id?.ToString() ?? string.Empty;
        if (query.TryGetValue("eventoNome", out var nome)) EventoNome = nome?.ToString() ?? string.Empty;
    }

    public async Task AtualizarAsync()
    {
        var result = await api.GetMapaMesasAsync(_eventoId);
        if (!result.Success || result.Data is null)
        {
            Aviso = result.Message ?? "Não foi possível carregar as mesas.";
            return;
        }

        Mesas.Clear();
        foreach (var m in result.Data) Mesas.Add(new MesaNoMapa(m));

        Aviso = result.Data.Count == 0
            ? result.Message ?? "Nenhuma mesa cadastrada para este salão. Peça ao gerente para montar o mapa no painel."
            : null;
        var prontos = result.Data.Sum(m => m.PedidosProntos);
        Resumo = prontos > 0
            ? $"{prontos} pedido(s) pronto(s) para buscar na cozinha"
            : $"{result.Data.Count(m => m.Situacao == "aguardando")} mesa(s) aguardando a cozinha";
    }

    [RelayCommand]
    private Task AbrirMesaAsync(MesaNoMapa mesa)
        => Shell.Current.GoToAsync("garcom-mesa", new Dictionary<string, object>
        {
            ["eventoId"] = _eventoId,
            ["mesa"] = mesa.Mesa,
        });
}
