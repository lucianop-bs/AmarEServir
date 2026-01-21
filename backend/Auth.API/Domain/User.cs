using AmarEServir.Core.Entities;
using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;
using Auth.API.Domain.Enums;
using Auth.API.Domain.Errors;

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

                () => string.IsNullOrWhiteSpace(Name)
                    ? Result.Fail(UserErrors.Profile.NameRequired)
                    : Result.Ok(),

                () => string.IsNullOrWhiteSpace(Phone)
                    ? Result.Fail(UserErrors.Profile.PhoneRequired)
                    : Result.Ok(),

                () => string.IsNullOrWhiteSpace(Email)
                    ? Result.Fail(UserErrors.Account.EmailRequired)
                    : Result.Ok(),

                () => string.IsNullOrWhiteSpace(Password)
                    ? Result.Fail(UserErrors.Account.PasswordRequired)
                    : Result.Ok(),

                () => !Enum.IsDefined(Role)
                    ? Result.Fail(UserErrors.Account.RoleInvalid)
                    : Result.Ok(),

                () => Address == null
                    ? Result.Fail(UserErrors.Address.AddressRequired)
                    : Address.Validate()
            );

            if (!resultValidation.IsSuccess)
                return Result.Fail(resultValidation.Errors);

            return Result.Ok();
        }

        public void UserUpdate(string? name, string? email, string? phone, Address? address, UserRole? role)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;
            if (!string.IsNullOrWhiteSpace(email))
                Email = email;
            if (!string.IsNullOrWhiteSpace(phone))
                Phone = phone;
            if (address != null)
                
                Address = Address.Update(address);
            if (role != null)
                Role = role.Value;

            SetUpdatedAtDate(DateTime.UtcNow);
        }
    }
}

