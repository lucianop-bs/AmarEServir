using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using Auth.API.Infrastructure.Persistence.Context;
using MongoDB.Driver;

namespace Auth.API.Infrastructure.Persistence.Repositories
{
    public class CellRepository : ICellRepository
    {
        private readonly IMongoCollection<Cell> _collection;
        public CellRepository(MongoContext context)
        {
            _collection = context.Database.GetCollection<Cell>("Cell");
        }

        public async Task Create(IClientSessionHandle session, Cell cell)
        {
            await _collection.InsertOneAsync(session, cell);
        }

        public async Task Create(Cell cell)
        {
            await _collection.InsertOneAsync(cell);
        }

        public async Task Delete(Guid id)
        {
            await _collection.DeleteOneAsync(c => c.Id == id);
        }

        public async Task<Cell> GetCellByGuid(Guid id)
        {
            return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(Cell cell)
        {
            await _collection.ReplaceOneAsync(c => c.Id == cell.Id, cell);
        }
    }
}
