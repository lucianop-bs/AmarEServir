using Auth.API.Api.Configurations;
using Auth.API.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.API.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        private readonly JsonWebTokenHandler _tokenHandler = new();

        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(User user)

        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),

                [JwtRegisteredClaimNames.Name] = user.Name,

                [JwtRegisteredClaimNames.Email] = user.Email,

                [ClaimTypes.Role] = user.Role.ToString(),

                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),

                [JwtRegisteredClaimNames.Iat] = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,

                Audience = _jwtSettings.Audience,

                Claims = claims,

                NotBefore = DateTime.UtcNow,

                IssuedAt = DateTime.UtcNow,

                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),

                SigningCredentials = credentials
            };

            return _tokenHandler.CreateToken(tokenDescriptor);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}