using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using Auth.API.Infrastructure.Persistence.Context;
using MongoDB.Driver;

namespace Auth.API.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(MongoContext context)
        {
            _collection = context.Database.GetCollection<User>("User");
        }

        public async Task Create(IClientSessionHandle session, User user)
        {
            await _collection.InsertOneAsync(session, user);
        }

        public async Task Delete(Guid id)
        {
            await _collection.DeleteOneAsync(u => u.Id == id);
        }

        public async Task Update(User user)
        {
            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<User> GetUserByGuid(Guid? id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public async Task<User> GetUserByEmail(string? email)
        {
            return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public Task<bool> EmailExistsForAnotherUser(string? email, Guid? currentUserId)
        {
            return _collection.Find(u => u.Email == email && u.Id != currentUserId).AnyAsync();
        }
    }
}