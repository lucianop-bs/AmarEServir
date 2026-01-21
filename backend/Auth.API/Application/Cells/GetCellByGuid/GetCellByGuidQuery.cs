using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Dtos;
using MediatR;

namespace Auth.API.Application.Cells.GetCellByGuid
{
    public record class GetCellByGuidQuery(Guid Id) : IRequest<Result<CellResponseDto>>;

}
