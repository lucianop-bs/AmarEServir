using AmarEServir.Core.Entities;
using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;

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
            var resultValidation = ResultValidation.ValidateCollectErrors(
                () => string.IsNullOrWhiteSpace(Name) || Name.Length < 4 || Name.Length > 50
                ? Result.Fail(CellError.InvalidName)
                : Result.Ok(),

                () => LeaderId == Guid.Empty || LeaderId is null
                ? Result.Fail(CellError.LeaderRequired)
                : Result.Ok()
                );

            if (!resultValidation.IsSuccess)
                return Result.Fail(resultValidation.Errors);

            return Result.Ok();
        }

        public Cell Update(string name, Guid? leaderId, User membro)
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                Name = name;
            }
            if (leaderId != Guid.Empty || leaderId is not null)
            {

                if (Users.Any(u => u.Id != leaderId))

                {
                    Users.RemoveAt(Users.FindIndex(u => u.Id == LeaderId));
                    LeaderId = leaderId;
                    Users.Add(membro);
                    SetUpdatedAtDate(DateTime.UtcNow);

                }
            }

            return this;

        }
    }
}
