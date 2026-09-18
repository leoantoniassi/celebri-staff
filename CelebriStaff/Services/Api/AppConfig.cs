namespace CelebriStaff.Services.Api;

public static class AppConfig
{
    /// <summary>
    /// Base da API do celebri (backend Node/Express, ver docker-compose.yml do
    /// repositório celebri — porta 3001). 10.0.2.2 é o alias que o emulador
    /// Android usa para "localhost" da máquina host.
    /// </summary>
    public static string ApiBaseUrl =>
#if ANDROID
        "http://10.0.2.2:3001/api/";
#else
        "http://localhost:3001/api/";
#endif
}
