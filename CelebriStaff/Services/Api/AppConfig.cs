using System.Reflection;

namespace CelebriStaff.Services.Api;

public static class AppConfig
{
    /// <summary>
    /// URL da API configurada na build (propriedade MSBuild <c>CelebriApiUrl</c>,
    /// definida em CelebriStaff/Directory.Build.local.props — não versionado).
    /// Usada para o túnel ngrok do repositório celebri: o celular chega na API
    /// de qualquer rede, sem cabo USB e sem <c>adb reverse</c>.
    /// </summary>
    private static readonly string? UrlConfigurada = LerUrlConfigurada();

    /// <summary>
    /// Base da API do celebri (backend Node/Express, porta 3001). Com
    /// <c>CelebriApiUrl</c> definida, ela vale para qualquer dispositivo.
    /// Sem ela, o padrão local: 10.0.2.2 (alias do "localhost" do PC no
    /// emulador Android), 127.0.0.1 no celular físico (exige
    /// <c>adb reverse tcp:3001 tcp:3001</c> pelo cabo) e localhost no Windows.
    /// </summary>
    public static string ApiBaseUrl => UrlConfigurada ?? UrlLocalPadrao;

    private static string UrlLocalPadrao =>
#if ANDROID
        DeviceInfo.DeviceType == DeviceType.Virtual
            ? "http://10.0.2.2:3001/api/"
            : "http://127.0.0.1:3001/api/";
#else
        "http://localhost:3001/api/";
#endif

    private static string? LerUrlConfigurada()
    {
        var valor = typeof(AppConfig).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "CelebriApiUrl")?.Value?.Trim();

        if (string.IsNullOrWhiteSpace(valor)
            || !Uri.TryCreate(valor, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
            return null;

        // HttpClient só concatena caminhos relativos corretamente com "/" no fim.
        return valor.EndsWith('/') ? valor : valor + "/";
    }
}
