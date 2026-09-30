using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Services.Auth;
using CelebriStaff.Services.Theme;

namespace CelebriStaff.ViewModels;

/// <summary>Primeiro acesso: código recebido por e-mail + criação da senha.</summary>
public partial class PrimeiroAcessoViewModel : ObservableObject
{
    private readonly AuthService _authService;

    public PrimeiroAcessoViewModel(AuthService authService, ThemeService themeService)
    {
        _authService = authService;
        NomeFantasia = themeService.NomeFantasia;
        Instrucao = $"Enviamos um código de 6 dígitos para {authService.EmailEmAndamento}. Digite-o abaixo e crie sua senha.";
    }

    [ObservableProperty]
    private string nomeFantasia = string.Empty;

    [ObservableProperty]
    private string instrucao = string.Empty;

    [ObservableProperty]
    private string codigo = string.Empty;

    [ObservableProperty]
    private string senha = string.Empty;

    [ObservableProperty]
    private string confirmacaoSenha = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? infoMessage;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task CriarSenhaAsync()
    {
        InfoMessage = null;
        if (Codigo.Trim().Length != 6)
        {
            ErrorMessage = "Informe o código de 6 dígitos.";
            return;
        }
        if (Senha.Trim().Length < 6)
        {
            ErrorMessage = "A senha deve ter no mínimo 6 caracteres.";
            return;
        }
        if (Senha != ConfirmacaoSenha)
        {
            ErrorMessage = "As senhas não conferem.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var result = await _authService.ConfirmarPrimeiroAcessoAsync(Codigo.Trim(), Senha);
            if (!result.Success || result.Data is null)
            {
                ErrorMessage = result.Message ?? "Não foi possível criar a senha.";
                return;
            }

            await Shell.Current.GoToAsync("//home");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ReenviarCodigoAsync()
    {
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var result = await _authService.SolicitarCodigoAsync();
            if (result.Success) InfoMessage = "Enviamos um novo código.";
            else ErrorMessage = result.Message ?? "Não foi possível reenviar o código.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
