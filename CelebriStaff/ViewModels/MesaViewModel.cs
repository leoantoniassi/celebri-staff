using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Models;
using CelebriStaff.Services.Api;

namespace CelebriStaff.ViewModels;

/// <summary>Um pedido da mesa, com o que o garçom pode fazer com ele.</summary>
public class PedidoDaMesa(Pedido pedido)
{
    public Pedido Pedido { get; } = pedido;
    public string Itens => string.Join("\n", Pedido.Itens.Select(i => (i.Alerta ? "⚠ " : "") + i.Linha));
    public string Cabecalho => $"{Pedido.CriadoEm.ToLocalTime():HH\\:mm} · {Rotulo}";
    public bool PodeEntregar => Pedido.Status == "pronto";
    public bool PodeCancelar => Pedido.Status == "enviado";

    private string Rotulo => Pedido.Status switch
    {
        "enviado" => "Enviado à cozinha",
        "preparando" => "Em preparo",
        "pronto" => "PRONTO — buscar na cozinha",
        "entregue" => "Entregue",
        "cancelado" => "Cancelado",
        _ => Pedido.Status,
    };

    public Color Cor => Pedido.Status switch
    {
        "pronto" => Color.FromArgb("#1B7F3B"),
        "enviado" or "preparando" => Color.FromArgb("#E08A00"),
        _ => Colors.Gray,
    };
}

/// <summary>Comanda da mesa: pedidos já feitos e anotação de um pedido novo.</summary>
public partial class MesaViewModel(ApiClient api) : ObservableObject, IQueryAttributable
{
    private string _eventoId = string.Empty;
    private string _mesaId = string.Empty;

    [ObservableProperty]
    private string titulo = string.Empty;

    [ObservableProperty]
    private string descricao = string.Empty;

    [ObservableProperty]
    private int quantidade = 1;

    [ObservableProperty]
    private string observacaoItem = string.Empty;

    [ObservableProperty]
    private bool alerta;

    [ObservableProperty]
    private string? mensagem;

    [ObservableProperty]
    private bool temItens;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<PedidoItem> Novos { get; } = [];
    public ObservableCollection<PedidoDaMesa> Pedidos { get; } = [];

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("eventoId", out var id)) _eventoId = id?.ToString() ?? string.Empty;
        if (query.TryGetValue("mesa", out var obj) && obj is MesaMapa mesa)
        {
            _mesaId = mesa.Id;
            Titulo = $"Mesa {mesa.Numero} · {mesa.Lugares} lugares";
        }
    }

    public async Task AtualizarAsync()
    {
        var result = await api.GetPedidosDaMesaAsync(_eventoId, _mesaId);
        if (!result.Success || result.Data is null) return;
        Pedidos.Clear();
        foreach (var p in result.Data) Pedidos.Add(new PedidoDaMesa(p));
    }

    [RelayCommand]
    private void Mais() => Quantidade = Math.Min(Quantidade + 1, 100);

    [RelayCommand]
    private void Menos() => Quantidade = Math.Max(Quantidade - 1, 1);

    [RelayCommand]
    private void AdicionarItem()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
        {
            Mensagem = "Escreva o item (ex.: suco de uva, mais salgados).";
            return;
        }
        Novos.Add(new PedidoItem
        {
            Descricao = Descricao.Trim(),
            Quantidade = Quantidade,
            Observacao = string.IsNullOrWhiteSpace(ObservacaoItem) ? null : ObservacaoItem.Trim(),
            Alerta = Alerta,
        });
        Descricao = ObservacaoItem = string.Empty;
        Quantidade = 1;
        Alerta = false;
        Mensagem = null;
        TemItens = true;
    }

    [RelayCommand]
    private void RemoverItem(PedidoItem item)
    {
        Novos.Remove(item);
        TemItens = Novos.Count > 0;
    }

    [RelayCommand]
    private async Task EnviarAsync()
    {
        if (Novos.Count == 0 || IsBusy) return;
        IsBusy = true;
        try
        {
            var result = await api.CriarPedidoAsync(_eventoId, _mesaId, Novos, null);
            if (!result.Success)
            {
                Mensagem = result.Message ?? "Não foi possível enviar o pedido.";
                return;
            }
            Novos.Clear();
            TemItens = false;
            Mensagem = "Pedido enviado para a cozinha!";
            await AtualizarAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private Task EntregarAsync(PedidoDaMesa pedido) => MudarStatusAsync(pedido, "entregue");

    [RelayCommand]
    private async Task CancelarAsync(PedidoDaMesa pedido)
    {
        if (await Shell.Current.DisplayAlertAsync("Cancelar pedido", "Avisar a cozinha que este pedido foi cancelado?", "Cancelar pedido", "Voltar"))
            await MudarStatusAsync(pedido, "cancelado");
    }

    private async Task MudarStatusAsync(PedidoDaMesa pedido, string status)
    {
        var result = await api.MudarStatusPedidoAsync(_eventoId, pedido.Pedido.Id, status);
        if (!result.Success) Mensagem = result.Message ?? "Não foi possível atualizar o pedido.";
        await AtualizarAsync();
    }
}
