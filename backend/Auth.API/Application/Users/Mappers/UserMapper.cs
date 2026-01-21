using Auth.API.Application.Users.Dtos;
using Auth.API.Domain;

namespace Auth.API.Application.Users.Mappers
{
    public static class UserMapper
    {
        public static UserResponseDto ToResponseDto(this User user)
        {
            if (user is null) return null!;

            return new UserResponseDto(
                user.Id,
                user.Name,
                user.Email,
                user.Phone,
                user.Address.ToResponseDto(),
                user.Role
            );
        }

        public static User ToDomain(this CreateUserRequestDto dto)
        {
            return new User(
                dto.Name,
                dto.Email,
                dto.Phone,
                dto.Password,
                dto.Address,
                dto.Role
            );
        }
    }
}
