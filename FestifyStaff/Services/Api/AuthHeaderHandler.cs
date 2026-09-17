using System.Net;
using System.Net.Http.Headers;
using FestifyStaff.Services.Auth;

namespace FestifyStaff.Services.Api;

/// <summary>
/// Injeta "Authorization: Bearer &lt;token&gt;" em toda chamada autenticada e
/// limpa a sessão local em caso de 401 (token expirado/inválido). A tela que
/// disparou a chamada é responsável por perceber o 401 e voltar ao login.
/// </summary>
public class AuthHeaderHandler(ITokenStore tokenStore) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await tokenStore.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await tokenStore.ClearTokenAsync();
        }

        return response;
    }
}
