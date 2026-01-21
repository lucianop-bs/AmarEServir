namespace Auth.API.Application.Cells.Dtos
{
    public record class UpdateCellRequestDto(string? Name, Guid? LeaderId);
}
