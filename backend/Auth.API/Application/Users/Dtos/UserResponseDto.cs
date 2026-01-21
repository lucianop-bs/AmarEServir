using Auth.API.Domain.Enums;

namespace Auth.API.Application.Users.Dtos
{
    public record class UserResponseDto(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        AddressResponseDto Address,
        UserRole Role
    );
}
