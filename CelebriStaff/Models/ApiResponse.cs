namespace CelebriStaff.Models;

/// <summary>
/// Espelha o envelope padrão de resposta da API Celebri: { success, message?, data? }.
/// Ver celebri/backend/src/utils/response.js.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    /// <summary>
    /// Só preenchido pela resposta de POST /api/auth/login — o JWT vem como
    /// campo irmão de "data", não aninhado nele.
    /// </summary>
    public string? Token { get; set; }
}
