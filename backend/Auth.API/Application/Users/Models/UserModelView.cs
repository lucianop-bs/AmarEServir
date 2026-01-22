using Auth.API.Domain;
using Auth.API.Domain.Enums;

namespace Auth.API.Application.Users.Models
{
    public record class UserResponse(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        AddressResponse Address,
        UserRole Role
        );
    public record class AddressResponse(
        string Rua,
        string Quadra,
        string Numero,
        string Bairro,
        string Cidade,
        string Estado,
        string Complemento,
        string Cep
        );

    public static class UserModelViewExtension
    {
        public static UserResponse ToResponse(this User user)
        {
            if (user is null) return null!;
            return new UserResponse(
                 user.Id,
                 user.Name,
                 user.Email,
                 user.Phone,
                 user.Address.ToResponse(),
                 user.Role
                 );
        }

        public static AddressResponse ToResponse(this Address address)
        {
            if (address is null) return null!;
            return new AddressResponse(
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