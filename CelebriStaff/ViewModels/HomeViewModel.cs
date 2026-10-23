using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Services.Api;
using CelebriStaff.Services.Auth;
using CelebriStaff.Services.Feedback;

namespace CelebriStaff.ViewModels;

/// <summary>"Minhas escalas": próximos eventos, confirmação de presença e check-in.</summary>
public partial class HomeViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly ApiClient _api;
    private readonly IFeedbackService _feedback;

    public HomeViewModel(AuthService authService, ApiClient api, IFeedbackService feedback)
    {
        _authService = authService;
        _api = api;
        _feedback = feedback;
    }

    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    [ObservableProperty]
    private bool isRefreshing;

    /// <summary>Só na primeira carga: mostra o skeleton em vez de uma lista vazia piscando.</summary>
    [ObservableProperty]
    private bool carregandoInicial = true;

    [ObservableProperty]
    private string? aviso;

    public ObservableCollection<GrupoEscalas> Grupos { get; } = [];

    [RelayCommand]
    private async Task CarregarAsync()
    {
        IsRefreshing = true;
        // A página da home é reaproveitada pelo Shell entre logins.
        WelcomeMessage = _authService.CurrentUser is { } user ? $"Olá, {user.Nome.Split(' ')[0]}!" : "Bem-vindo(a)!";
        try
        {
            var result = await _api.GetMinhasEscalasAsync();
            Grupos.Clear();

            if (!result.Success || result.Data is null)
            {
                Aviso = result.Message ?? "Não foi possível carregar suas escalas.";
                return;
            }

            var agora = DateTimeOffset.Now;
            var cards = result.Data
                .Where(e => e.Evento is not null && e.Evento.Status != "cancelado")
                .Select(e => new EscalaCardViewModel(e, agora))
                .ToList();

            var proximas = cards.Where(c => !c.Terminou).OrderBy(c => c.Escala.Evento!.DataEvento).ToList();
            var passadas = cards.Where(c => c.Terminou).OrderByDescending(c => c.Escala.Evento!.DataEvento).Take(20).ToList();

            if (proximas.Count > 0) Grupos.Add(new GrupoEscalas("Próximos eventos", proximas));
            if (passadas.Count > 0) Grupos.Add(new GrupoEscalas("Eventos anteriores", passadas));

            Aviso = cards.Count == 0
                ? result.Message ?? "Você ainda não foi escalado em nenhum evento."
                : null;
        }
        finally
        {
            IsRefreshing = false;
            CarregandoInicial = false;
        }
    }

    [RelayCommand]
    private Task ConfirmarAsync(EscalaCardViewModel card) => ResponderAsync(card, "confirmado");

    [RelayCommand]
    private async Task RecusarAsync(EscalaCardViewModel card)
    {
        var ok = await Shell.Current.DisplayAlertAsync(
            "Recusar escala", $"Avisar que você não vai trabalhar em \"{card.NomeEvento}\"?", "Recusar", "Voltar");
        if (ok) await ResponderAsync(card, "recusado");
    }

    private async Task ResponderAsync(EscalaCardViewModel card, string resposta)
    {
        var result = await _api.ResponderEscalaAsync(card.Escala.Id, resposta);
        if (!result.Success)
        {
            _feedback.Error(result.Message ?? "Não foi possível responder.");
            return;
        }
        card.Escala.Confirmacao = resposta;
        card.AtualizarEstado();
        if (resposta == "confirmado") _feedback.Success("Presença confirmada!");
        else _feedback.Info("Recusa registrada.");
    }

    [RelayCommand]
    private async Task CheckinAsync(EscalaCardViewModel card)
    {
        var result = await _api.CheckinEscalaAsync(card.Escala.Id);
        if (!result.Success)
        {
            _feedback.Error(result.Message ?? "Não foi possível fazer o check-in.");
            return;
        }
        card.Escala.CheckinEm = DateTimeOffset.Now;
        card.Escala.Confirmacao = "confirmado";
        card.AtualizarEstado();
        _feedback.Success("Check-in feito. Bom trabalho!");
    }

    [RelayCommand]
    private async Task AbrirModuloAsync(EscalaCardViewModel card)
    {
        var rota = card.Modulo switch
        {
            "portaria" => "portaria",
            "garcom" => "garcom",
            "cozinha" => "cozinha",
            _ => null,
        };
        if (rota is null) return;

        await Shell.Current.GoToAsync(rota, new Dictionary<string, object>
        {
            ["eventoId"] = card.Escala.EventoId,
            ["eventoNome"] = card.NomeEvento,
        });
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await Shell.Current.GoToAsync("//entrar");
    }
}
