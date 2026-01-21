using Auth.API.Domain.Enums;

namespace Auth.API.Application.Users.Dtos
{
    public record class UpdateUserRequestDto(
        string? Name,
        string? Email,
        string? Phone,
        AddressRequestDto? Address,
        UserRole? Role
    );
}
