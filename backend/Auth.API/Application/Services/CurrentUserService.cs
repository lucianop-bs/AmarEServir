using System.Security.Claims;

namespace Auth.API.Application.Services;

/// <summary>
/// ═══════════════════════════════════════════════════════════════
/// IMPLEMENTAÇÃO DO SERVIÇO DE USUÁRIO ATUAL
/// ═══════════════════════════════════════════════════════════════
/// 
/// COMO FUNCIONA:
/// 
///  Request com Token
///        │
///        ▼
///  ┌─────────────────────┐
///  │ UseAuthentication() │  ← Valida token e preenche HttpContext.User
///  └─────────────────────┘
///        │
///        ▼
///  ┌─────────────────────┐
///  │ CurrentUserService  │  ← Lê dados do HttpContext.User
///  │                     │
///  │ UserId = "123..."   │
///  │ UserName = "João"   │
///  │ UserRole = "Leader" │
///  └─────────────────────┘
///        │
///        ▼
///  Seu Handler/Service usa os dados
/// 
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Pega o ClaimsPrincipal do usuário atual.
    /// É preenchido automaticamente pelo middleware de autenticação.
    /// </summary>
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    /// <summary>
    /// ID do usuário (extraído da claim "sub" ou "NameIdentifier").
    /// </summary>
    public Guid? UserId
    {
        get
        {
            var userIdString = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User?.FindFirst("sub")?.Value;

            if (Guid.TryParse(userIdString, out var userId))
                return userId;

            return null;
        }
    }

    /// <summary>
    /// Nome do usuário (extraído da claim "name" ou "Name").
    /// </summary>
    public string? UserName
        => User?.FindFirst(ClaimTypes.Name)?.Value
        ?? User?.FindFirst("name")?.Value;

    /// <summary>
    /// Email do usuário (extraído da claim "email" ou "Email").
    /// </summary>
    public string? UserEmail
        => User?.FindFirst(ClaimTypes.Email)?.Value
        ?? User?.FindFirst("email")?.Value;

    /// <summary>
    /// Role do usuário (Leader, Member, Admin).
    /// </summary>
    public string? UserRole
        => User?.FindFirst(ClaimTypes.Role)?.Value
        ?? User?.FindFirst("role")?.Value;

    /// <summary>
    /// Verifica se o usuário está autenticado.
    /// </summary>
    public bool IsAuthenticated
        => User?.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// Verifica se o usuário tem uma role específica.
    /// </summary>
    public bool IsInRole(string role)
    {
        return User?.IsInRole(role) ?? false;
    }
}