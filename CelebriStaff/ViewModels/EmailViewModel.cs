using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CelebriStaff.Models;
using CelebriStaff.Services.Auth;
using CelebriStaff.Services.Theme;

namespace CelebriStaff.ViewModels;

/// <summary>
/// Primeira tela: o funcionário informa só o e-mail e o app descobre em qual
/// buffet ele está cadastrado. Se estiver em mais de um, ele escolhe na lista.
/// </summary>
public partial class EmailViewModel(AuthService authService, ThemeService themeService) : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<BuffetIdentificado> Buffets { get; } = [];

    /// <summary>Se já existe sessão salva, reaplica o tema e pula direto para a home.</summary>
    public async Task RestoreSessionIfAnyAsync()
    {
        if (!await authService.TryRestoreSessionAsync()) return;

        if (authService.TenantSlug is { } slug)
        {
            var tenant = await authService.ResolveTenantAsync(slug);
            if (tenant is { Success: true, Data: not null })
            {
                themeService.Apply(tenant.Data);
            }
        }

        await Shell.Current.GoToAsync("//home");
    }

    [RelayCommand]
    private async Task ContinueAsync()
    {
        var emailInformado = Email.Trim();
        if (string.IsNullOrEmpty(emailInformado))
        {
            ErrorMessage = "Informe seu e-mail.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        Buffets.Clear();
        try
        {
            var result = await authService.IdentificarAsync(emailInformado);
            if (!result.Success || result.Data is null)
            {
                ErrorMessage = result.Message ?? "Não foi possível verificar o e-mail.";
                return;
            }

            if (result.Data.Count == 0)
            {
                ErrorMessage = "E-mail não cadastrado em nenhum buffet. Fale com o responsável pela sua equipe.";
                return;
            }

            authService.EmailEmAndamento = emailInformado;

            if (result.Data.Count == 1)
            {
                await EntrarNoBuffetAsync(result.Data[0]);
                return;
            }

            foreach (var buffet in result.Data) Buffets.Add(buffet);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EscolherBuffetAsync(BuffetIdentificado buffet)
    {
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            await EntrarNoBuffetAsync(buffet);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task EntrarNoBuffetAsync(BuffetIdentificado buffet)
    {
        var tenant = await authService.ResolveTenantAsync(buffet.Slug);
        if (!tenant.Success || tenant.Data is null)
        {
            ErrorMessage = tenant.Message ?? "Empresa não encontrada.";
            return;
        }
        themeService.Apply(tenant.Data);

        if (!buffet.PrimeiroAcesso)
        {
            await Shell.Current.GoToAsync("login");
            return;
        }

        var codigo = await authService.SolicitarCodigoAsync();
        if (!codigo.Success)
        {
            ErrorMessage = codigo.Message ?? "Não foi possível enviar o código.";
            return;
        }
        await Shell.Current.GoToAsync("primeiro-acesso");
    }
}
