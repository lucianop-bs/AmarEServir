using AmarEServir.Core.Entities;
using AmarEServir.Core.Results.Base;

namespace Auth.API.Domain
{
    public class Cell : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid? LeaderId { get; private set; }
        public User Lider { get; private set; }
        public List<User> Users { get; private set; } = [];

        public Cell() { }

        public Cell(
            string name,
            Guid leaderId,
            User membro,
            Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Name = name;
            LeaderId = leaderId;
            Users.Add(membro);
        }

        public Result Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return Result.Fail(CellError.InvalidName);
            }
            if (Name.Length < 4 || Name.Length > 50)
            {
                return Result.Fail(CellError.InvalidNameLength);
            }
            if (LeaderId == Guid.Empty || LeaderId is null)
            {
                return Result.Fail(CellError.LeaderRequired);
            }

            return Result.Ok();
        }

        public Result Update(string name, Guid? leaderId, User membro)
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                Name = name;
            }
            if (leaderId != Guid.Empty || leaderId is not null)
            {

                if (Users.Any(u => u.Id == leaderId))

                {
                    Users.RemoveAt(Users.FindIndex(u => u.Id == LeaderId));
                    LeaderId = leaderId;
                    Users.Add(membro);
                    SetUpdatedAtDate(DateTime.UtcNow);

                }
            }

            return Result.Ok();

        }
    }
}
