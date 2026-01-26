using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using MediatR;

namespace Auth.API.Application.Cells.Commands.CreateCell
{
    public record class CreateCellCommand(CreateCellRequest Cell) : IRequest<Result<CellResponse>>;
    public record class CreateCellRequest(
        string? Name,
        Guid? LeaderId
        );
}