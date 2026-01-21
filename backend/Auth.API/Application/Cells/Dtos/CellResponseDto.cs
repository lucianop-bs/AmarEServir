using Auth.API.Application.Users.Dtos;

namespace Auth.API.Application.Cells.Dtos
{
    public record class CellResponseDto(
        Guid Id,
        string Name,
        string LeaderId,
        List<UserResponseDto> Members
    );
}
