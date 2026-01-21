using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Dtos;
using MediatR;

namespace Auth.API.Application.Cells.CreateCell
{
    public record class CreateCellCommand(CreateCellRequestDto Cell) : IRequest<Result<CellResponseDto>>;
}
