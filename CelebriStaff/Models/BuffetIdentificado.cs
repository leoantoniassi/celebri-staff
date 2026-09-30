namespace CelebriStaff.Models;

/// <summary>Um buffet em que o e-mail está cadastrado (POST auth/identificar).</summary>
public class BuffetIdentificado
{
    public string Slug { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public bool PrimeiroAcesso { get; set; }
}
