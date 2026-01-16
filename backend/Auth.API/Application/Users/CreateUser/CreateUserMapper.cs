using Auth.API.Domain;

namespace Auth.API.Application.Users.CreateUser
{
    public static class CreateUserMapper
    {
        public static User ToDomain(this CreateUserCommand command, Guid IdCell, Guid IdUsuario)
        {
            return new User(command.Name,
                command.Email, command.Phone, command.Password, command.ToAddressDomain(), IdCell, command.CellName!, command.Role, IdUsuario
                );
        }

        public static Address ToAddressDomain(this CreateUserCommand command)
        {
            return new Address(command.Address.Rua, command.Address.Quadra, command.Address.Numero, command.Address.Bairro, command.Address.Cidade, command.Address.Estado, command.Address.Complemento, command.Address.Cep);
        }
    }
}
