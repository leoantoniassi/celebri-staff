namespace FestifyStaff.Models;

/// <summary>
/// Item de GET /api/escala/minhas — uma alocação do funcionário logado em um evento.
/// Ver festify/backend/src/controllers/escalaController.js (listarMinhas).
/// </summary>
public class Escala
{
    public string Id { get; set; } = string.Empty;
    public string EventoId { get; set; } = string.Empty;
    public string FuncionarioId { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public EscalaEvento? Evento { get; set; }
}

public class EscalaEvento
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTime DataEvento { get; set; }
    public DateTime HorarioTermino { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? LocalId { get; set; }
}
