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
        public List<User> Members { get; private set; } = [];

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
            Members.Add(membro);
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

        public Result<Cell> Update(string name, Guid? leaderId, User member)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
            if (leaderId.HasValue && leaderId.Value != Guid.Empty)
            {
                if (member is null) return Result<Cell>.Fail(CellError.NotFound);

                if (LeaderId != leaderId)
                {
                    var index = Members.FindIndex(getMember => getMember.Id == LeaderId);
                    if (index != -1)
                    {
                        Members.RemoveAt(index);

                    }
                }
                LeaderId = leaderId;
                if (!Members.Any(getMember => getMember.Id == member.Id))

                {

                    Members.Add(member);
                }
            }
            SetUpdatedAtDate(DateTime.UtcNow);
            var validation = Validate();

            if (!validation.IsSuccess)
                return Result<Cell>.Fail(validation.Errors);

            return Result<Cell>.Ok(this);

        }
    }
}
