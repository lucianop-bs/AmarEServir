using AmarEServir.Core.Results.Base;
using MediatR;

namespace Auth.API.Application.Cells.UpdateCell
{
    public record class UpdateCellCommand(Guid Id, string Name, Guid? LiderId) : IRequest<Result>;
    public record class UpdateCellRequest(string Name, Guid? LiderId);

}
