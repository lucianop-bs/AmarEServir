using Auth.API.Domain;

namespace Auth.API.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}