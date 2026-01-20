using AmarEServir.Core.Entities;
using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;
using Auth.API.Domain.Errors;

namespace Auth.API.Domain
{
    public class Cell : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid LeaderId { get; private set; }
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

                () => LeaderId == Guid.Empty
                ? Result.Fail(CellError.LeaderRequired)
                : Result.Ok()
                );

            if (!resultValidation.IsSuccess)
                return Result.Fail(resultValidation.Errors);

            return Result.Ok();
        }

        public void Update(string name, Guid leaderId, User member)
        {

            if (!string.IsNullOrWhiteSpace(name)) Name = name;

            if (LeaderId != leaderId)
            {

                RemoveMemberById(LeaderId);
                LeaderId = leaderId;
                AddMember(member);

            }
            SetUpdatedAtDate(DateTime.UtcNow);
        }

        private void AddMember(User member)
        {
            if (member != null && !Members.Any(m => m.Id == member.Id))
            {
                Members.Add(member);
            }
        }

        private void RemoveMemberById(Guid memberId)
        {
            var member = Members.FirstOrDefault(m => m.Id == memberId);
            if (member != null)
            {
                Members.Remove(member);
            }
        }
    }
}
