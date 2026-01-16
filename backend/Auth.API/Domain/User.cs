using AmarEServir.Core.Entities;
using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;

namespace Auth.API.Domain
{
    public class User : BaseEntity<Guid>
    {

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Password { get; private set; }
        public Address Address { get; private set; }

        public UserRole Role { get; private set; }
        public User() { }

        public User(
            string name,
            string email,
            string phone,
            string password,
            Address address,
            UserRole role,
            Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Name = name;
            Email = email;
            Phone = phone;
            Password = password;
            Address = address;
            Role = role;
        }

        public Result Validate()
        {
            var resultValidation = ResultValidation.ValidateCollectErrors(
                () => string.IsNullOrWhiteSpace(Name) || Name.Length < 3 || Name.Length > 50
                ? Result.Fail(string.IsNullOrWhiteSpace(Name) ? UserError.NameRequired : UserError.NameLength)
                : Result.Ok(),

                () => string.IsNullOrWhiteSpace(Email) || !Email.Contains('@')
                ? Result.Fail(UserError.InvalidEmail)
                : Result.Ok(),

                () => string.IsNullOrWhiteSpace(Phone) || Phone.Length > 13 || Phone.Length < 11
               ? Result.Fail(string.IsNullOrWhiteSpace(Phone) ? UserError.PhoneRequired : UserError.PhoneInvalid)
               : Result.Ok(),

                 () => string.IsNullOrWhiteSpace(Password) || Password.Length <= 6
               ? Result.Fail(UserError.WeakPassword)
               : Result.Ok(),

               () => !Enum.IsDefined(Role)
               ? Result.Fail(UserError.RoleInvalid)
               : Result.Ok(),

               () => Address == null
               ? Result.Fail(UserError.AddressRequired)
               : Address.Validate()
               );

            if (!resultValidation.IsSuccess)
                return Result.Fail(resultValidation.Errors);

            return Result.Ok();

        }
    }
}

