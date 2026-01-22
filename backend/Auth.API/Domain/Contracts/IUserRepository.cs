namespace Auth.API.Domain.Contracts
{
    public interface IUserRepository
    {
        Task Create(User user);

        Task Update(User user);

        Task Delete(Guid id);

        Task<User> GetUserByGuid(Guid? id);

        Task<User> GetUserByEmail(string? email);

        Task<bool> EmailExistsForAnotherUser(string? email, Guid? currentUserId);
    }
}