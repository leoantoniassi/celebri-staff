namespace CelebriStaff.Services.Api;

public static class AppConfig
{
    /// <summary>
    /// Só para testar o celular físico por Wi-Fi: IP do PC na rede (ipconfig),
    /// com a API subida com API_BIND=0.0.0.0. Deixe null para o padrão: celular
    /// no cabo USB com `adb reverse tcp:3001 tcp:3001`, que leva o 127.0.0.1
    /// do celular até a API no PC — aí o IP do PC não importa.
    /// </summary>
    private const string? LanHost = null;

    /// <summary>
    /// Base da API do celebri (backend Node/Express, ver docker-compose.yml do
    /// repositório celebri — porta 3001). 10.0.2.2 é o alias que o emulador
    /// Android usa para "localhost" da máquina host.
    /// </summary>
    public static string ApiBaseUrl =>
#if ANDROID
        DeviceInfo.DeviceType == DeviceType.Virtual
            ? "http://10.0.2.2:3001/api/"
            : string.IsNullOrWhiteSpace(LanHost)
                ? "http://127.0.0.1:3001/api/"
                : $"http://{LanHost}:3001/api/";
#else
        "http://localhost:3001/api/";
#endif
}
