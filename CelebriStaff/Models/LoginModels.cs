namespace CelebriStaff.Models;

/// <summary>Corpo de POST /api/auth/login.</summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

/// <summary>
/// Resposta de POST /api/auth/login (campo "data").
/// Ver celebri/backend/src/controllers/authController.js.
/// </summary>
public class LoginResult
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? EmpresaId { get; set; }
    public EmpresaResumo? Empresa { get; set; }

    /// <summary>
    /// Preenchido apenas quando esta conta está vinculada a um registro de
    /// Funcionario (colaborador de campo) — ver migration 006.
    /// </summary>
    public string? FuncionarioId { get; set; }
    public FuncionarioResumo? Funcionario { get; set; }
}

public class EmpresaResumo
{
    public string Id { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}

public class FuncionarioResumo
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Funcao { get; set; }
}
