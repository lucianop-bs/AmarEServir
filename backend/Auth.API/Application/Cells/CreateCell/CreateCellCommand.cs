using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Models;
using MediatR;

namespace Auth.API.Application.Cells.CreateCell
{
    public record class CreateCellCommand(CreateCellRequest Cell) : IRequest<Result<CellModelView>>;
    public record class CreateCellRequest(string? Name, Guid? LeaderId);
}
