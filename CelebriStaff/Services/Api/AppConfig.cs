namespace CelebriStaff.Services.Api;

public static class AppConfig
{
    /// <summary>
    /// IP do PC que roda o backend, na mesma rede Wi-Fi do celular físico
    /// (usado só no Android real; o emulador usa 10.0.2.2). Troque se o IP
    /// do PC mudar (ipconfig).
    /// </summary>
    private const string LanHost = "192.168.1.14";

    /// <summary>
    /// Base da API do celebri (backend Node/Express, ver docker-compose.yml do
    /// repositório celebri — porta 3001). 10.0.2.2 é o alias que o emulador
    /// Android usa para "localhost" da máquina host.
    /// </summary>
    public static string ApiBaseUrl =>
#if ANDROID
        DeviceInfo.DeviceType == DeviceType.Virtual
            ? "http://10.0.2.2:3001/api/"
            : $"http://{LanHost}:3001/api/";
#else
        "http://localhost:3001/api/";
#endif
}
