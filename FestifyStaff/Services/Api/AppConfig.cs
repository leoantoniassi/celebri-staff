namespace FestifyStaff.Services.Api;

public static class AppConfig
{
    /// <summary>
    /// Base da API do festify (backend Node/Express, ver docker-compose.yml do
    /// repositório festify — porta 3001). 10.0.2.2 é o alias que o emulador
    /// Android usa para "localhost" da máquina host.
    /// </summary>
    public static string ApiBaseUrl =>
#if ANDROID
        "http://10.0.2.2:3001/api/";
#else
        "http://localhost:3001/api/";
#endif
}
