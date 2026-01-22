using System.Security.Claims;

namespace Auth.API.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

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

    public string? UserName
        => User?.FindFirst(ClaimTypes.Name)?.Value
        ?? User?.FindFirst("name")?.Value;

    public string? UserEmail
        => User?.FindFirst(ClaimTypes.Email)?.Value
        ?? User?.FindFirst("email")?.Value;

    public string? UserRole
        => User?.FindFirst(ClaimTypes.Role)?.Value
        ?? User?.FindFirst("role")?.Value;

    public bool IsAuthenticated
        => User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role)
    {
        return User?.IsInRole(role) ?? false;
    }
}