using Auth.API.Domain;

namespace Auth.API.Application.Users.CreateUser
{
    public static class CreateUserMapper
    {
        public static User ToDomain(this CreateUserCommand command)
        {

            return new User(command.Name, command.Email, command.Phone, command.Password, command.Address, command.Role);
        }
    }
}
