namespace CelebriStaff.Services.Auth;

/// <summary>
/// Abstrai a persistência do JWT. Existe como interface separada do
/// AuthService para o AuthHeaderHandler (que injeta o token em toda
/// requisição HTTP) não precisar depender do AuthService inteiro.
/// </summary>
public interface ITokenStore
{
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string token);
    Task ClearTokenAsync();
}

public class SecureTokenStore : ITokenStore
{
    private const string TokenKey = "auth_token";

    public Task<string?> GetTokenAsync() => SecureStorage.Default.GetAsync(TokenKey);

    public Task SetTokenAsync(string token) => SecureStorage.Default.SetAsync(TokenKey, token);

    public Task ClearTokenAsync()
    {
        SecureStorage.Default.Remove(TokenKey);
        return Task.CompletedTask;
    }
}
