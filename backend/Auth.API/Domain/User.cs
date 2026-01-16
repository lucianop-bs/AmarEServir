using AmarEServir.Core.Entities;
using AmarEServir.Core.Results.Base;

namespace Auth.API.Domain
{
    public class User : BaseEntity<Guid>
    {

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Password { get; private set; }
        public Address Address { get; private set; }
        public Guid? CellId { get; private set; }
        public string? CellName { get; private set; }
        public UserRole Role { get; private set; }
        public User() { }

        public User(
            string name,
            string email,
            string phone,
            string password,
            Address address,
            Guid? celulaId,
            string cellName,
            UserRole role,
            Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Name = name;
            Email = email;
            Phone = phone;
            Password = password;
            Address = address;
            CellId = celulaId;
            CellName = cellName;
            Role = role;
        }

        public Result Validate()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 3 || Name.Length > 50)
                return Result.Fail(UserError.NameRequired);

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
                return Result.Fail(UserError.InvalidEmail);

            if (string.IsNullOrWhiteSpace(Phone))
                return Result.Fail(UserError.PhoneInvalid);

            if (string.IsNullOrWhiteSpace(Password) || Password.Length <= 6)
                return Result.Fail(UserError.WeakPassword);

            if (!Enum.IsDefined(Role))
                return Result.Fail(UserError.TypeInvalid);
            if (Role == UserRole.Leader && string.IsNullOrWhiteSpace(CellName) && CellId == Guid.Empty)
                return Result.Fail(UserError.RoleRequired);

            if ((Role != UserRole.Leader) && !string.IsNullOrWhiteSpace(CellName))
                return Result.Fail(UserError.InvalidForRole);

            var addressValidation = Address.Validate();

            if (!addressValidation.IsSuccess)
                return addressValidation;

            return Result.Ok();
        }
    }
}

