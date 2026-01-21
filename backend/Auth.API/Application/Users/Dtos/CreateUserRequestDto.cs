using Auth.API.Domain;
using Auth.API.Domain.Enums;

namespace Auth.API.Application.Users.Dtos
{
    public record class CreateUserRequestDto(
        string Name,
        string Email,
        string Phone,
        string Password,
        UserRole Role,
        Address Address
    );
}
