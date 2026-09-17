using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FestifyStaff.Services.Auth;
using FestifyStaff.Services.Theme;

namespace FestifyStaff.ViewModels;

/// <summary>Primeira tela: colaborador informa o código (slug) do buffet — US01.</summary>
public partial class TenantCodeViewModel(AuthService authService, ThemeService themeService) : ObservableObject
{
    [ObservableProperty]
    private string companyCode = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBusy;

    /// <summary>
    /// Chamado quando a tela aparece: se já existe uma sessão salva (login
    /// anterior), pula direto para a home em vez de pedir o código de novo.
    /// </summary>
    public async Task RestoreSessionIfAnyAsync()
    {
        if (!await authService.TryRestoreSessionAsync()) return;

        // Reaplica o white label do tenant salvo antes de pular pra home —
        // sem isso a tela reabriria com a paleta neutra até o próximo login.
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
        var slug = CompanyCode.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(slug))
        {
            ErrorMessage = "Informe o código da sua empresa.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var result = await authService.ResolveTenantAsync(slug);
            if (!result.Success || result.Data is null)
            {
                ErrorMessage = result.Message ?? "Empresa não encontrada.";
                return;
            }

            themeService.Apply(result.Data);
            await Shell.Current.GoToAsync("login");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
