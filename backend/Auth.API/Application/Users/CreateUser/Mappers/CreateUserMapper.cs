using Auth.API.Domain;

namespace Auth.API.Application.Users.CreateUser.Mappers
{
    public static class CreateUserMapper
    {
        public static User ToUser(this CreateUserCommand command)
        {

            return new User(
                command.User.Name,
                command.User.Email,
                command.User.Phone,
                command.User.Password,
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
}

