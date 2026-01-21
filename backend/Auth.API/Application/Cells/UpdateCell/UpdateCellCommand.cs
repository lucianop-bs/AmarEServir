using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Dtos;
using MediatR;

namespace Auth.API.Application.Cells.UpdateCell
{
    public record class UpdateCellCommand(Guid Id, UpdateCellRequestDto Cell) : IRequest<Result>;

}
