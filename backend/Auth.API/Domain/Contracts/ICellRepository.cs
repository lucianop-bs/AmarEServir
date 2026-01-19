using MongoDB.Driver;

namespace Auth.API.Domain.Contracts
{
    public interface ICellRepository
    {
        Task Create(IClientSessionHandle session, Cell cell);
        Task Update(Cell cell);
        Task Delete(Guid id);
        Task<Cell> GetCellByGuid(Guid id);
        Task Create(Cell cell);

    }
}
