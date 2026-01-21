namespace Auth.API.Application.Cells.Dtos
{
    public record class CreateCellRequestDto(string? Name, Guid? LeaderId);
}
