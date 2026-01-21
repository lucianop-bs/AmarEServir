using Auth.API.Domain;

namespace Auth.API.Application.Users.CreateUser
{
    public static class CreateUserMapper
    {
        public static User ToDomain(this CreateUserCommand command)
        {

            return new User(command.User.Name, command.User.Email, command.User.Phone, command.User.Password, command.User.Address, command.User.Role);
        }
    }
}
