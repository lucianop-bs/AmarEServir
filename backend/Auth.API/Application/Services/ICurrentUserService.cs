namespace Auth.API.Application.Services
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// ID do usuário logado (Guid).
        /// Retorna null se não estiver autenticado.
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// Nome do usuário logado.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Email do usuário logado.
        /// </summary>
        string? UserEmail { get; }

        /// <summary>
        /// Role do usuário logado (Leader, Member, Admin).
        /// </summary>
        string? UserRole { get; }

        /// <summary>
        /// Verifica se o usuário está autenticado.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Verifica se o usuário tem uma role específica.
        /// </summary>
        bool IsInRole(string role);
    }
}
