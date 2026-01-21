using Auth.API.Application.Cells.Contracts;
using Auth.API.Application.Users.Models;
using Auth.API.Domain;

namespace Auth.API.Application.Cells.Contracts
{
    public record class CellResponse(Guid Id, string Name, string LeaderId, List<UserResponse> Members);

    public static class CellModelViewExtension
    {
        public static CellResponse ToResponse(this Cell cell)
        {
            return new CellResponse(
                cell.Id,
                cell.Name,
                cell.LeaderId.ToString(),
                cell.Members?.Select(user => user.ToResponse()).ToList() ?? []

                );
        }
    }
}
