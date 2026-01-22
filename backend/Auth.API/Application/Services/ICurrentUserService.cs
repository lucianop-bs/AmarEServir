namespace Auth.API.Application.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }

        string? UserName { get; }

        string? UserEmail { get; }

        string? UserRole { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(string role);
    }
}