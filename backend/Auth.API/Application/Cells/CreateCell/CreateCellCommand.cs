using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Models;
using MediatR;

namespace Auth.API.Application.Cells.CreateCell
{

    public record class CreateCellCommand(string Name, Guid LeaderId) : IRequest<Result<CellModelView>>;

}
