using System.Text.Json;
using CelebriStaff.Models;
using CelebriStaff.Services.Api;

namespace CelebriStaff.Services.Auth;

/// <summary>
/// Coordena o fluxo de tenant + login: guarda o slug da empresa (Preferences,
/// não é sensível), o JWT (SecureStorage, via ITokenStore) e os dados do
/// usuário logado (SecureStorage — pode conter dados pessoais).
/// </summary>
public class AuthService(ApiClient api, ITokenStore tokenStore)
{
    private const string SlugKey = "tenant_slug";
    private const string UserKey = "current_user";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public string? TenantSlug
    {
        get
        {
            var value = Preferences.Default.Get(SlugKey, string.Empty);
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }

    public LoginResult? CurrentUser { get; private set; }

    /// <summary>E-mail digitado na primeira tela, usado pelas telas de senha e de primeiro acesso.</summary>
    public string EmailEmAndamento { get; set; } = string.Empty;

    public Task<ApiResponse<List<BuffetIdentificado>>> IdentificarAsync(string email, CancellationToken ct = default)
        => api.IdentificarAsync(email, ct);

    public Task<ApiResponse<object>> SolicitarCodigoAsync(CancellationToken ct = default)
        => api.SolicitarCodigoPrimeiroAcessoAsync(TenantSlug ?? string.Empty, EmailEmAndamento, ct);

    public async Task<ApiResponse<LoginResult>> ConfirmarPrimeiroAcessoAsync(string codigo, string senha, CancellationToken ct = default)
    {
        var result = await api.ConfirmarPrimeiroAcessoAsync(TenantSlug ?? string.Empty, EmailEmAndamento, codigo, senha, ct);
        await SalvarSessaoAsync(result);
        return result;
    }

    /// <summary>Valida o código da empresa e guarda o slug para as chamadas seguintes.</summary>
    public async Task<ApiResponse<TenantConfig>> ResolveTenantAsync(string slug, CancellationToken ct = default)
    {
        var result = await api.GetTenantConfigAsync(slug, ct);
        if (result.Success)
        {
            Preferences.Default.Set(SlugKey, slug);
        }
        return result;
    }

    public async Task<ApiResponse<LoginResult>> LoginAsync(string email, string senha, CancellationToken ct = default)
    {
        var slug = TenantSlug;
        if (string.IsNullOrWhiteSpace(slug))
        {
            return new ApiResponse<LoginResult>
            {
                Success = false,
                Message = "Código da empresa não informado. Volte e informe o código do seu buffet.",
            };
        }

        var result = await api.LoginAsync(slug, email, senha, ct);
        await SalvarSessaoAsync(result);
        return result;
    }

    private async Task SalvarSessaoAsync(ApiResponse<LoginResult> result)
    {
        if (result is not { Success: true, Data: not null, Token: not null }) return;

        await tokenStore.SetTokenAsync(result.Token);
        CurrentUser = result.Data;
        await SecureStorage.Default.SetAsync(UserKey, JsonSerializer.Serialize(result.Data, JsonOptions));
    }

    /// <summary>Restaura a sessão salva (chamado na inicialização do app).</summary>
    public async Task<bool> TryRestoreSessionAsync()
    {
        var token = await tokenStore.GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return false;

        var userJson = await SecureStorage.Default.GetAsync(UserKey);
        if (string.IsNullOrEmpty(userJson)) return false;

        CurrentUser = JsonSerializer.Deserialize<LoginResult>(userJson, JsonOptions);
        return CurrentUser is not null;
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        await tokenStore.ClearTokenAsync();
        SecureStorage.Default.Remove(UserKey);
    }
}
