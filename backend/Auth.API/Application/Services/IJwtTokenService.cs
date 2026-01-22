using Auth.API.Domain;

namespace Auth.API.Application.Services
{
    /// <summary>
    /// Interface para o serviço de geração de tokens JWT.
    /// Seguindo o princípio de inversão de dependência (SOLID - D)
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Gera um token JWT para o usuário informado.
        /// </summary>
        /// <param name="user">Usuário autenticado</param>
        /// <returns>Token JWT como string</returns>
        string GenerateToken(User user);
    }
}
