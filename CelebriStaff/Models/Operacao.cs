namespace CelebriStaff.Models;

/// <summary>Mesa no mapa do garçom (GET operacao/eventos/:id/mesas).</summary>
public class MesaMapa
{
    public string Id { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public int Lugares { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }
    public int PedidosAbertos { get; set; }
    public int PedidosProntos { get; set; }

    /// <summary>livre | aguardando | pronto</summary>
    public string Situacao { get; set; } = "livre";
}

public class PedidoItem
{
    public string? Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; } = 1;
    public string? Observacao { get; set; }
    public bool Alerta { get; set; }

    public string Linha => Observacao is { Length: > 0 } obs
        ? $"{Quantidade}× {Descricao} — {obs}"
        : $"{Quantidade}× {Descricao}";
}

public class Pedido
{
    public string Id { get; set; } = string.Empty;
    public string MesaId { get; set; } = string.Empty;
    public string? Mesa { get; set; }

    /// <summary>enviado | preparando | pronto | entregue | cancelado</summary>
    public string Status { get; set; } = string.Empty;
    public string? Observacao { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public List<PedidoItem> Itens { get; set; } = [];
    public bool TemAlerta { get; set; }
}

public class ServicoCardapio
{
    public string Id { get; set; } = string.Empty;
    public string Item { get; set; } = string.Empty;
    public decimal? Quantidade { get; set; }
    public string? Unidade { get; set; }
    public DateTimeOffset? Horario { get; set; }

    /// <summary>pendente | preparando | servido</summary>
    public string Status { get; set; } = "pendente";
    public string? Observacao { get; set; }
}

public class ItemEstoque
{
    public string ProdutoId { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Unidade { get; set; }
    public decimal ReservadoParaEvento { get; set; }
    public decimal EmEstoque { get; set; }
    public decimal EstoqueMinimo { get; set; }
    public bool EstoqueBaixo { get; set; }
}

public class EventoCozinha
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int QtdPessoas { get; set; }
    public int QtdAdultos { get; set; }
    public int QtdCriancas { get; set; }
}

/// <summary>GET operacao/eventos/:id/cozinha — tudo que a tela da cozinha mostra.</summary>
public class PainelCozinha
{
    public EventoCozinha Evento { get; set; } = new();
    public List<ServicoCardapio> Servicos { get; set; } = [];
    public List<Pedido> Pedidos { get; set; } = [];
    public List<ItemEstoque> Estoque { get; set; } = [];
}
