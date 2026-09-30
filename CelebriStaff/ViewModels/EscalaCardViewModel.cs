using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CelebriStaff.Models;

namespace CelebriStaff.ViewModels;

/// <summary>Um cartão da lista de escalas, com o que a tela precisa já calculado.</summary>
public partial class EscalaCardViewModel : ObservableObject
{
    private static readonly CultureInfo PtBr = new("pt-BR");
    private static readonly TimeSpan AntecedenciaCheckin = TimeSpan.FromHours(3);

    public EscalaCardViewModel(Escala escala, DateTimeOffset agora)
    {
        Escala = escala;
        var evento = escala.Evento;
        var inicio = evento?.DataEvento.ToLocalTime() ?? DateTimeOffset.MinValue;
        var fim = evento?.HorarioTermino.ToLocalTime() ?? DateTimeOffset.MinValue;

        NomeEvento = evento?.Nome ?? "Evento";
        Quando = $"{inicio.ToString("ddd, dd/MM", PtBr)} · {inicio:HH\\:mm}–{fim:HH\\:mm}";
        Local = evento?.Local?.Nome;
        Funcao = escala.Funcao?.Nome ?? "Sem função definida";
        Modulo = escala.Funcao?.Modulo;
        Terminou = fim <= agora;
        JanelaDoEvento = agora >= inicio - AntecedenciaCheckin && agora <= fim;
        AtualizarEstado();
    }

    public Escala Escala { get; }
    public string NomeEvento { get; }
    public string Quando { get; }
    public string? Local { get; }
    public string Funcao { get; }
    public string? Modulo { get; }
    public bool Terminou { get; }

    /// <summary>Do check-in (3h antes) até o fim do evento.</summary>
    public bool JanelaDoEvento { get; }

    [ObservableProperty]
    private string status = string.Empty;

    [ObservableProperty]
    private Color statusCor = Colors.Gray;

    [ObservableProperty]
    private bool podeResponder;

    [ObservableProperty]
    private bool podeConfirmar;

    [ObservableProperty]
    private bool podeRecusar;

    [ObservableProperty]
    private bool podeFazerCheckin;

    [ObservableProperty]
    private bool podeAbrirModulo;

    /// <summary>Módulos com tela registrada no AppShell.</summary>
    public static readonly HashSet<string> ModulosDisponiveis = [];

    public string TextoModulo => Modulo is null || !ModulosDisponiveis.Contains(Modulo) ? string.Empty : Modulo switch
    {
        "portaria" => "Abrir Portaria",
        "garcom" => "Abrir Garçom",
        "cozinha" => "Abrir Cozinha",
        _ => string.Empty,
    };

    public void AtualizarEstado()
    {
        (Status, StatusCor) = Escala switch
        {
            { CheckinEm: { } chegada } => ($"Chegou às {chegada.ToLocalTime():HH\\:mm}", Color.FromArgb("#1B7F3B")),
            { Confirmacao: "confirmado" } => ("Presença confirmada", Color.FromArgb("#1B7F3B")),
            { Confirmacao: "recusado" } => ("Você recusou", Color.FromArgb("#B3261E")),
            _ when Terminou => ("Sem resposta", Colors.Gray),
            _ => ("Aguardando sua resposta", Color.FromArgb("#B26A00")),
        };

        PodeResponder = !Terminou && Escala.CheckinEm is null;
        PodeConfirmar = PodeResponder && Escala.Confirmacao != "confirmado";
        PodeRecusar = PodeResponder && Escala.Confirmacao != "recusado";
        PodeFazerCheckin = JanelaDoEvento && Escala.CheckinEm is null && Escala.Confirmacao != "recusado";
        PodeAbrirModulo = JanelaDoEvento && !string.IsNullOrEmpty(TextoModulo) && Escala.Confirmacao != "recusado";
    }
}

public class GrupoEscalas(string titulo, IEnumerable<EscalaCardViewModel> itens) : List<EscalaCardViewModel>(itens)
{
    public string Titulo { get; } = titulo;
}
