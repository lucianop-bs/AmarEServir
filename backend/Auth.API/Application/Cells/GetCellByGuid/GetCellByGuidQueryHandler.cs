using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Contracts;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Cells.GetCellByGuid;

public interface IGetCellByGuidQueryHandler : IRequestHandler<GetCellByGuidQuery, Result<CellResponse>>
{ }

public class GetCellByGuidQueryHandler : IGetCellByGuidQueryHandler
{
    private readonly ICellRepository _cellRepository;

    public GetCellByGuidQueryHandler(ICellRepository cellRepository)
    {
        _cellRepository = cellRepository;
    }

    public async Task<Result<CellResponse>> Handle(GetCellByGuidQuery request, CancellationToken cancellationToken)
    {
        var cell = await _cellRepository.GetCellByGuid(request.Id);

        if (cell is null)
            return Result<CellResponse>.Fail(CellError.NotFound);

        var validacaoCell = cell.Validate();

        if (!validacaoCell.IsSuccess)
            return Result<CellResponse>.Fail(validacaoCell.Errors);

        return Result<CellResponse>.Ok(cell.ToResponse());
    }
}