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

        Task<bool> LeaderExists(Guid leader);
        Task<bool> NameAlreadyExists(string name);
        Task<bool> NameExistsForAnotherCell(string name, Guid? currentCellId);
        Task<bool> LeaderExistsForAnotherCell(Guid? leaderId, Guid currentCellId);

    }
}
