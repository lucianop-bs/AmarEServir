using Auth.API.Application.Common.Models;
using Auth.API.Domain;

namespace Auth.API.Application.Users.Commands.CreateUser
{
    public static class CreateUserMapper
    {
        public static User ToUser(this CreateUserCommand command, string hashPassword)
        {
            return new User(
                command.User.Name,
                command.User.Email,
                command.User.Phone,
                hashPassword,
                command.User.Address.ToAddress(),
                command.User.Role);
        }

        public static Address ToAddress(this AddressRequest request)
        {
            return new Address(
                request.Rua,
                request.Quadra,
                request.Numero,
                request.Bairro,
                request.Cidade,
                request.Estado,
                request.Complemento,
                request.Cep
            );
        }
    }

    public static class UserModelResponseExtension
    {
        public static UserResponse ToUserResponse(this User user)
        {
            if (user is null) return null!;
            return new UserResponse(
                 user.Id,
                 user.Name,
                 user.Email,
                 user.Phone,
                 user.Address.ToAddressResponse(),
                 user.Role
                 );
        }

        public static AddressResponse ToAddressResponse(this Address address)
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
