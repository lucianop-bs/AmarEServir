using Auth.API.Application.Cells.Dtos;
using Auth.API.Application.Users.Mappers;
using Auth.API.Domain;

namespace Auth.API.Application.Cells.Mappers
{
    public static class CellMapper
    {
        public static CellResponseDto ToResponseDto(this Cell cell)
        {
            return new CellResponseDto(
                cell.Id,
                cell.Name,
                cell.LeaderId.ToString(),
                cell.Members?.Select(user => user.ToResponseDto()).ToList() ?? []
            );
        }
    }
}
