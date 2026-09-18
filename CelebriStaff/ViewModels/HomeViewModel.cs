using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Services.Auth;

namespace CelebriStaff.ViewModels;

/// <summary>
/// Placeholder pós-login — só prova o fluxo ponta a ponta (tenant -> login ->
/// sessão restaurada). O conteúdo real de "Minhas Escalas" é próxima etapa.
/// </summary>
public partial class HomeViewModel : ObservableObject
{
    private readonly AuthService _authService;

    public HomeViewModel(AuthService authService)
    {
        _authService = authService;
        WelcomeMessage = _authService.CurrentUser is { } user
            ? $"Olá, {user.Nome}!"
            : "Bem-vindo(a)!";
    }

    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await Shell.Current.GoToAsync("//tenant");
    }
}
