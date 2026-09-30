namespace CelebriStaff.Models;

/// <summary>
/// Item de GET /api/escala/minhas — uma alocação do funcionário logado em um evento.
/// Ver celebri/backend/src/controllers/escalaController.js (listarMinhas).
/// </summary>
public class Escala
{
    public string Id { get; set; } = string.Empty;
    public string EventoId { get; set; } = string.Empty;
    public string FuncionarioId { get; set; } = string.Empty;
    public string? Observacoes { get; set; }

    /// <summary>pendente | confirmado | recusado</summary>
    public string Confirmacao { get; set; } = "pendente";
    public DateTimeOffset? CheckinEm { get; set; }

    /// <summary>Função neste evento (a da escala ou, sem ela, a do cadastro).</summary>
    public FuncaoResumo? Funcao { get; set; }
    public EscalaEvento? Evento { get; set; }
}

public class EscalaEvento
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTimeOffset DataEvento { get; set; }
    public DateTimeOffset HorarioTermino { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? LocalId { get; set; }
    public LocalResumo? Local { get; set; }
}

public class FuncaoResumo
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;

    /// <summary>cozinha | portaria | garcom | null</summary>
    public string? Modulo { get; set; }
}

public class LocalResumo
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}
