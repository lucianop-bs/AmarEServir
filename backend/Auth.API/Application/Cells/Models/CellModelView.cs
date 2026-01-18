using Auth.API.Application.Users.Models;
using Auth.API.Domain;

namespace Auth.API.Application.Cells.Models
{
    public record class CellModelView(Guid Id, string Name, string LeaderId, List<UserModelView> Members);

    public static class CellModelViewExtension
    {
        public static CellModelView ToModelView(this Cell cell)
        {
            return new CellModelView(
                cell.Id,
                cell.Name,
                cell.LeaderId.ToString(),
                cell.Users?.Select(user => user.ToModelUserView()).ToList() ?? []

                );
        }
    }
}
