namespace CelebriStaff.Models;

/// <summary>GET portaria/eventos/:id — números da entrada do evento.</summary>
public class PortariaResumo
{
    public string EventoId { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public bool UsaConvites { get; set; }
    public int QtdPessoasEvento { get; set; }
    public int Convites { get; set; }
    public int PessoasConvidadas { get; set; }
    public int EntraramComConvite { get; set; }
    public int Avulsos { get; set; }
    public int TotalPresentes { get; set; }
}

/// <summary>Convite como a portaria vê: 1 pessoa ou um grupo com o mesmo QR.</summary>
public class ConvitePortaria
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int QtdPessoas { get; set; }
    public int QtdEntrou { get; set; }
    public int Excedente { get; set; }

    public string Situacao => QtdEntrou == 0
        ? $"{QtdPessoas} pessoa(s) · ninguém entrou"
        : $"Entraram {QtdEntrou} de {QtdPessoas}";
}
