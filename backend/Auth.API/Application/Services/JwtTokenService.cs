using Auth.API.Api.Configurations;
using Auth.API.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;
using System.Text;

namespace Auth.API.Application.Services
{
    /// <summary>
    /// Serviço responsável por gerar tokens JWT.
    /// 
    /// COMO FUNCIONA:
    /// 1. Recebe um usuário válido
    /// 2. Cria "claims" (informações sobre o usuário)
    /// 3. Assina o token com a chave secreta
    /// 4. Retorna o token como string
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        // ✅ NOVO: JsonWebTokenHandler é thread-safe e pode ser reutilizado
        private readonly JsonWebTokenHandler _tokenHandler = new();
        /// <summary>
        /// Construtor que recebe as configurações via Injeção de Dependência.
        /// O IOptions<T> é um padrão do ASP.NET para ler configurações tipadas.
        /// </summary>
        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GenerateToken(User user)

        {
            // ═══════════════════════════════════════════════════════════════
            // PASSO 1: Criar a CHAVE DE ASSINATURA
            // ═══════════════════════════════════════════════════════════════
            //
            // A chave secreta é usada para "assinar" o token.
            // É como o carimbo oficial que prova que o crachá é verdadeiro.
            //
            // Encoding.UTF8.GetBytes() converte a string em bytes
            // SymmetricSecurityKey cria uma chave simétrica (mesma chave para assinar e verificar)
            //
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            // SigningCredentials = Credenciais de assinatura
            // HmacSha256 = Algoritmo de hash (o mesmo que definimos no Header do JWT)

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // ═══════════════════════════════════════════════════════════════
            // PASSO 2: Criar as CLAIMS (informações que vão dentro do token)
            // ═══════════════════════════════════════════════════════════════
            // 
            // Claims são "afirmações" sobre o usuário.
            // É como preencher os campos de um crachá:
            //   - Nome: João Silva
            //   - Cargo: Líder
            //   - ID: 12345
            //

            var claims = new Dictionary<string, object>
            {
                // ID do usuário (subject)
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),

                // Nome do usuário
                [JwtRegisteredClaimNames.Name] = user.Name,

                // Email do usuário
                [JwtRegisteredClaimNames.Email] = user.Email,

                // Role - Usado pelo [Authorize(Roles = "...")]
                [ClaimTypes.Role] = user.Role.ToString(),

                // ID único do token (para invalidação se necessário)
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),

                // Data de emissão do token
                [JwtRegisteredClaimNames.Iat] = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()

            };

            // ═══════════════════════════════════════════════════════════════
            // PASSO 3: Criar o SECURITY TOKEN DESCRIPTOR
            // ═══════════════════════════════════════════════════════════════
            //
            // ✅ NOVO: SecurityTokenDescriptor é mais limpo e performático
            // Ele descreve COMO o token deve ser criado
            //
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Quem emitiu o token (sua API)
                Issuer = _jwtSettings.Issuer,

                // Para quem o token é destinado (seu frontend)
                Audience = _jwtSettings.Audience,

                // Informações do usuário
                Claims = claims,

                // Válido a partir de agora
                NotBefore = DateTime.UtcNow,

                // Quando foi emitido
                IssuedAt = DateTime.UtcNow,

                // Quando expira
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),

                // Credenciais de assinatura
                SigningCredentials = credentials
            };

            // ═══════════════════════════════════════════════════════════════
            // PASSO 4: Criar o TOKEN
            // ═══════════════════════════════════════════════════════════════
            //
            // ✅ NOVO: CreateToken retorna a string diretamente
            // Não precisa mais de WriteToken()
            //
            return _tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
