namespace FestifyStaff.Models;

/// <summary>
/// Resposta de GET /api/tenant/config — identidade visual white label do buffet.
/// Ver festify/backend/src/controllers/tenantConfigController.js.
/// </summary>
public class TenantConfig
{
    public string Id { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public TenantCores Cores { get; set; } = new();
}

public class TenantCores
{
    public string Primaria { get; set; } = "#FEDC57";
    public string Secundaria { get; set; } = "#7DBA00";
    public string Terciaria { get; set; } = "#6600A1";
}
