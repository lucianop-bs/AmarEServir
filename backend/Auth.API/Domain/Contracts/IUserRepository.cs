using MongoDB.Driver;

namespace Auth.API.Domain.Contracts
{
    public interface IUserRepository
    {
        Task Create(IClientSessionHandle session, User user);
        Task Create(User user);
        Task Update(User user);
        Task Delete(Guid id);
        Task<User> GetUserByGuid(Guid id);

    }
}
