using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Models;
using CelebriStaff.Services.Api;

namespace CelebriStaff.ViewModels;

public class PedidoNaCozinha(Pedido pedido)
{
    public Pedido Pedido { get; } = pedido;
    public string Cabecalho => $"Mesa {Pedido.Mesa} · {Pedido.CriadoEm.ToLocalTime():HH\\:mm}";
    public string Itens => string.Join("\n", Pedido.Itens.Select(i => (i.Alerta ? "⚠ ALERGIA: " : "") + i.Linha));
    public string Status => Pedido.Status switch
    {
        "enviado" => "Novo",
        "preparando" => "Em preparo",
        "pronto" => "Pronto — aguardando garçom",
        _ => Pedido.Status,
    };
    public Color Cor => Pedido.TemAlerta ? Color.FromArgb("#B3261E") : Color.FromArgb("#DDDDDD");
    public bool PodeComecar => Pedido.Status == "enviado";
    public bool PodeFinalizar => Pedido.Status is "enviado" or "preparando";
    public bool PodeReabrir => Pedido.Status == "pronto";
}

public class ServicoNaCozinha(ServicoCardapio servico)
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public ServicoCardapio Servico { get; } = servico;
    public string Horario => Servico.Horario?.ToLocalTime().ToString("HH:mm") ?? "—";
    public string Item => Servico.Item;
    public string Quantidade => Servico.Quantidade is { } q ? $"{q.ToString("0.##", PtBr)} {Servico.Unidade}" : "Quantidade livre";
    public string Status => Servico.Status switch { "preparando" => "Em preparo", "servido" => "Servido", _ => "Pendente" };
    public string ProximaAcao => Servico.Status switch { "pendente" => "Começar preparo", "preparando" => "Marcar servido", _ => "Reabrir" };
    public string ProximoStatus => Servico.Status switch { "pendente" => "preparando", "preparando" => "servido", _ => "pendente" };
}

public class EstoqueNaCozinha(ItemEstoque item)
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public string Nome => item.Nome;
    public string Detalhe =>
        $"Reservado p/ evento: {item.ReservadoParaEvento.ToString("0.##", PtBr)} {item.Unidade} · " +
        $"em estoque: {item.EmEstoque.ToString("0.##", PtBr)} (mín. {item.EstoqueMinimo.ToString("0.##", PtBr)})";
    public bool Baixo => item.EstoqueBaixo;
}

/// <summary>Cozinha: pedidos dos garçons, cardápio com horários e estoque do evento.</summary>
public partial class CozinhaViewModel(ApiClient api) : ObservableObject, IQueryAttributable
{
    private string _eventoId = string.Empty;

    [ObservableProperty]
    private string eventoNome = string.Empty;

    [ObservableProperty]
    private string publico = string.Empty;

    [ObservableProperty]
    private string aba = "pedidos";

    [ObservableProperty]
    private string? erro;

    [ObservableProperty]
    private string tituloPedidos = "Pedidos";

    public bool AbaPedidos => Aba == "pedidos";
    public bool AbaCardapio => Aba == "cardapio";
    public bool AbaEstoque => Aba == "estoque";

    public ObservableCollection<PedidoNaCozinha> Pedidos { get; } = [];
    public ObservableCollection<ServicoNaCozinha> Servicos { get; } = [];
    public ObservableCollection<EstoqueNaCozinha> Estoque { get; } = [];

    partial void OnAbaChanged(string value)
    {
        OnPropertyChanged(nameof(AbaPedidos));
        OnPropertyChanged(nameof(AbaCardapio));
        OnPropertyChanged(nameof(AbaEstoque));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("eventoId", out var id)) _eventoId = id?.ToString() ?? string.Empty;
        if (query.TryGetValue("eventoNome", out var nome)) EventoNome = nome?.ToString() ?? string.Empty;
    }

    public async Task AtualizarAsync()
    {
        var result = await api.GetPainelCozinhaAsync(_eventoId);
        if (!result.Success || result.Data is null)
        {
            Erro = result.Message ?? "Não foi possível carregar a cozinha.";
            return;
        }
        Erro = null;
        var painel = result.Data;
        Publico = $"{painel.Evento.QtdPessoas} pessoas · {painel.Evento.QtdAdultos} adultos · {painel.Evento.QtdCriancas} crianças";

        Pedidos.Clear();
        foreach (var p in painel.Pedidos) Pedidos.Add(new PedidoNaCozinha(p));
        var novos = painel.Pedidos.Count(p => p.Status == "enviado");
        TituloPedidos = novos > 0 ? $"Pedidos ({novos} novo{(novos > 1 ? "s" : "")})" : "Pedidos";

        Servicos.Clear();
        foreach (var s in painel.Servicos) Servicos.Add(new ServicoNaCozinha(s));

        Estoque.Clear();
        foreach (var e in painel.Estoque.OrderByDescending(e => e.EstoqueBaixo)) Estoque.Add(new EstoqueNaCozinha(e));
    }

    [RelayCommand]
    private void MudarAba(string nova) => Aba = nova;

    [RelayCommand]
    private Task ComecarAsync(PedidoNaCozinha p) => MudarPedidoAsync(p, "preparando");

    [RelayCommand]
    private Task ProntoAsync(PedidoNaCozinha p) => MudarPedidoAsync(p, "pronto");

    [RelayCommand]
    private Task ReabrirAsync(PedidoNaCozinha p) => MudarPedidoAsync(p, "preparando");

    private async Task MudarPedidoAsync(PedidoNaCozinha p, string status)
    {
        var result = await api.MudarStatusPedidoAsync(_eventoId, p.Pedido.Id, status);
        if (!result.Success) Erro = result.Message;
        await AtualizarAsync();
    }

    [RelayCommand]
    private async Task AvancarServicoAsync(ServicoNaCozinha s)
    {
        var result = await api.MudarStatusServicoAsync(_eventoId, s.Servico.Id, s.ProximoStatus);
        if (!result.Success) Erro = result.Message;
        await AtualizarAsync();
    }
}
