using AmarEServir.Core.Entities;
using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;
using Auth.API.Domain.Enums;
using Auth.API.Domain.Errors;

namespace Auth.API.Domain
{
    public class Cell : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid? LeaderId { get; private set; }
        public User Lider { get; private set; }
        public List<User> Members { get; private set; } = [];

        public Cell()
        { }

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
                () => string.IsNullOrWhiteSpace(Name)
                ? Result.Fail(CellError.NameRequired)
                : Result.Ok(),

                () => LeaderId == Guid.Empty
                ? Result.Fail(CellError.LeaderRoleRequired)
                : Result.Ok()
                );

            if (!resultValidation.IsSuccess)
                return Result.Fail(resultValidation.Errors);

            return Result.Ok();
        }

        public static Result<Cell> Create(string name, User user)
        {
            if (user.Role != UserRole.Leader)
            {
                return Result<Cell>.Fail(CellError.LeaderRoleRequired);
            }

            var cell = new Cell(
                name: name,
                leaderId: user.Id,
                membro: user
            );

            var validationResult = cell.Validate();
            if (!validationResult.IsSuccess)
            {
                return Result<Cell>.Fail(validationResult.Errors);
            }

            return Result<Cell>.Ok(cell);
        }

        public Result Update(string name, Guid? leaderId, User member)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Fail(CellError.NameRequired);
            }

            if (member.Role != UserRole.Leader)
            {
                return Result.Fail(CellError.LeaderRoleRequired);
            }

            if (LeaderId != leaderId)
            {
                RemoveMemberById(LeaderId);
                LeaderId = leaderId;
                AddMember(member);
            }

            Name = name;
            SetUpdatedAtDate(DateTime.UtcNow);

            return Validate();
        }

        private void AddMember(User member)
        {
            if (member != null && !Members.Any(m => m.Id == member.Id))
            {
                Members.Add(member);
            }
        }

        private void RemoveMemberById(Guid? memberId)
        {
            var member = Members.FirstOrDefault(m => m.Id == memberId);
            if (member != null)
            {
                Members.Remove(member);
            }
        }
    }
}