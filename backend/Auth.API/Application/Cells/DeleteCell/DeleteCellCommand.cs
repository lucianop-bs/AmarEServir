using AmarEServir.Core.Results.Base;
using MediatR;

namespace Auth.API.Application.Cells.DeleteCell
{
    public record DeleteCellCommand(Guid Id) : IRequest<Result>;
}