using System.Net.Http.Json;
using System.Text.Json;
using CelebriStaff.Models;

namespace CelebriStaff.Services.Api;

/// <summary>
/// Cliente HTTP tipado para a API do celebri. Cada método devolve sempre um
/// ApiResponse&lt;T&gt; (nunca lança para erros de rede/HTTP esperados), para
/// as telas tratarem falha de conexão do mesmo jeito que erro de negócio.
/// </summary>
public class ApiClient(HttpClient http)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public Task<ApiResponse<TenantConfig>> GetTenantConfigAsync(string tenantSlug, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "tenant/config");
        request.Headers.Add("X-Tenant-Slug", tenantSlug);
        return SendAsync<TenantConfig>(request, ct);
    }

    public Task<ApiResponse<LoginResult>> LoginAsync(string tenantSlug, string email, string senha, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "auth/login")
        {
            Content = JsonContent.Create(new LoginRequest { Email = email, Senha = senha }, options: JsonOptions),
        };
        request.Headers.Add("X-Tenant-Slug", tenantSlug);
        return SendAsync<LoginResult>(request, ct);
    }

    public Task<ApiResponse<List<BuffetIdentificado>>> IdentificarAsync(string email, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "auth/identificar")
        {
            Content = JsonContent.Create(new { email }, options: JsonOptions),
        };
        return SendAsync<List<BuffetIdentificado>>(request, ct);
    }

    public Task<ApiResponse<object>> SolicitarCodigoPrimeiroAcessoAsync(string tenantSlug, string email, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "auth/primeiro-acesso/codigo")
        {
            Content = JsonContent.Create(new { email }, options: JsonOptions),
        };
        request.Headers.Add("X-Tenant-Slug", tenantSlug);
        return SendAsync<object>(request, ct);
    }

    public Task<ApiResponse<LoginResult>> ConfirmarPrimeiroAcessoAsync(
        string tenantSlug, string email, string codigo, string senha, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "auth/primeiro-acesso/confirmar")
        {
            Content = JsonContent.Create(new { email, codigo, senha }, options: JsonOptions),
        };
        request.Headers.Add("X-Tenant-Slug", tenantSlug);
        return SendAsync<LoginResult>(request, ct);
    }

    public Task<ApiResponse<List<Escala>>> GetMinhasEscalasAsync(CancellationToken ct = default)
    {
        // Rota autenticada: o tenant vem do JWT (AuthHeaderHandler já injeta
        // o Bearer token), não é preciso enviar X-Tenant-Slug aqui.
        var request = new HttpRequestMessage(HttpMethod.Get, "escala/minhas");
        return SendAsync<List<Escala>>(request, ct);
    }

    public Task<ApiResponse<object>> ResponderEscalaAsync(string escalaId, string resposta, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"escala/{escalaId}/confirmacao")
        {
            Content = JsonContent.Create(new { resposta }, options: JsonOptions),
        };
        return SendAsync<object>(request, ct);
    }

    public Task<ApiResponse<object>> CheckinEscalaAsync(string escalaId, CancellationToken ct = default)
        => SendAsync<object>(new HttpRequestMessage(HttpMethod.Post, $"escala/{escalaId}/checkin"), ct);

    private async Task<ApiResponse<T>> SendAsync<T>(HttpRequestMessage request, CancellationToken ct)
    {
        try
        {
            using var response = await http.SendAsync(request, ct);
            var body = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions, ct);

            if (body is not null) return body;

            return new ApiResponse<T>
            {
                Success = false,
                Message = $"Resposta inesperada do servidor (HTTP {(int)response.StatusCode}).",
            };
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Não foi possível conectar ao servidor. Verifique sua internet e tente novamente.",
            };
        }
    }
}
