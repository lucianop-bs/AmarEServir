using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Models;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using MediatR;

namespace Auth.API.Application.Cells.GetCellByGuid;

public interface IGetCellByGuidQueryHandler : IRequestHandler<GetCellByGuidQuery, Result<CellModelView>>
{ }

public class GetCellByGuidQueryHandler : IGetCellByGuidQueryHandler
{
    private readonly ICellRepository _cellRepository;
    public GetCellByGuidQueryHandler(ICellRepository cellRepository)
    {
        _cellRepository = cellRepository;
    }

    public async Task<Result<CellModelView>> Handle(GetCellByGuidQuery request, CancellationToken cancellationToken)
    {

        var cell = await _cellRepository.GetCellByGuid(request.Id);

        if (cell is null)
            return Result<CellModelView>.Fail(CellError.NotFound);

        var validacaoCell = cell.Validate();

        if (!validacaoCell.IsSuccess)
            return Result<CellModelView>.Fail(validacaoCell.Errors);

        return Result<CellModelView>.Ok(cell.ToModelView());

    }
}

