using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Services.Auth;
using CelebriStaff.Services.Theme;

namespace CelebriStaff.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;

    public LoginViewModel(AuthService authService, ThemeService themeService)
    {
        _authService = authService;
        NomeFantasia = themeService.NomeFantasia;
        LogoUrl = themeService.LogoUrl;
    }

    [ObservableProperty]
    private string nomeFantasia = string.Empty;

    [ObservableProperty]
    private string? logoUrl;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string senha = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
        {
            ErrorMessage = "Informe email e senha.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var result = await _authService.LoginAsync(Email.Trim(), Senha);
            if (!result.Success || result.Data is null)
            {
                ErrorMessage = result.Message ?? "Credenciais inválidas.";
                return;
            }

            await Shell.Current.GoToAsync("//home");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
