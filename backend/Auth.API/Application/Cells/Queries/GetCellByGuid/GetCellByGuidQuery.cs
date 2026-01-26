using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using MediatR;

namespace Auth.API.Application.Cells.Queries.GetCellByGuid
{
    public record class GetCellByGuidQuery(Guid Id) : IRequest<Result<CellResponse>>;
}