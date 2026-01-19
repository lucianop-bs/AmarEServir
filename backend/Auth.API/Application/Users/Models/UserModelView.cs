using Auth.API.Domain;
using Auth.API.Domain.Enums;

namespace Auth.API.Application.Users.Models
{
    public record class UserModelView(Guid Id, string Name, string Email, string Phone, AddressModelView Address, UserRole Role);
    public record class AddressModelView(string Rua, string Quadra, string Numero, string Bairro, string Cidade, string Estado, string Complemento, string Cep);

    public static class UserModelViewExtension
    {

        public static UserModelView ToModelUserView(this User user)
        {
            if (user is null) return null!;
            return new UserModelView(
                 user.Id,
                 user.Name,
                 user.Email,
                 user.Phone,
                 user.Address.ToModelAddress(),
                 user.Role
                 );

        }
        public static AddressModelView ToModelAddress(this Address address)
        {
            if (address is null) return null!;
            return new AddressModelView(
                 address.Rua,
                 address.Quadra,
                 address.Numero,
                 address.Bairro,
                 address.Cidade,
                 address.Estado,
                 address.Complemento,
                 address.Cep
            );
        }
    }
}

