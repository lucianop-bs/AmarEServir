using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Dtos;
using Auth.API.Application.Cells.Mappers;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Cells.GetCellByGuid;

public interface IGetCellByGuidQueryHandler : IRequestHandler<GetCellByGuidQuery, Result<CellResponseDto>>
{ }

public class GetCellByGuidQueryHandler : IGetCellByGuidQueryHandler
{
    private readonly ICellRepository _cellRepository;
    public GetCellByGuidQueryHandler(ICellRepository cellRepository)
    {
        _cellRepository = cellRepository;
    }

    public async Task<Result<CellResponseDto>> Handle(GetCellByGuidQuery request, CancellationToken cancellationToken)
    {

        var cell = await _cellRepository.GetCellByGuid(request.Id);

        if (cell is null)
            return Result<CellResponseDto>.Fail(CellError.NotFound);

        var validacaoCell = cell.Validate();

        if (!validacaoCell.IsSuccess)
            return Result<CellResponseDto>.Fail(validacaoCell.Errors);

        return Result<CellResponseDto>.Ok(cell.ToResponseDto());

    }
}

