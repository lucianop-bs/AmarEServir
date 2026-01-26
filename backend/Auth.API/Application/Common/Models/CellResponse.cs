using Auth.API.Application.Users.Commands.CreateUser;
using Auth.API.Domain;

namespace Auth.API.Application.Common.Models
{
    public record class CellResponse(
        Guid Id,
        string Name,
        Guid? LeaderId,
        List<UserResponse> Members
        );

    public static class CellModelViewExtension
    {
        public static CellResponse ToResponse(this Cell cell)
        {
            return new CellResponse(
                cell.Id,
                cell.Name,
                cell.LeaderId,
                cell.Members?.Select(user => user.ToUserResponse()).ToList() ?? []
                );
        }
    }
}