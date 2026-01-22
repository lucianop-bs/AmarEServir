using AmarEServir.Core.Results.Base;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Cells.DeleteCell
{
    public interface IDeleteCellCommandHandler : IRequestHandler<DeleteCellCommand, Result>
    {
    };

    public class DeleteCellCommandHandler : IDeleteCellCommandHandler

    {
        private readonly ICellRepository _cellRepository;

        public DeleteCellCommandHandler(ICellRepository cellRepository)
        {
            _cellRepository = cellRepository;
        }

        public async Task<Result> Handle(DeleteCellCommand request, CancellationToken cancellationToken)
        {
            var cell = await _cellRepository.GetCellByGuid(request.Id);
            if (cell is null)
            {
                return Result.Fail(CellError.NotFound);
            }

            await _cellRepository.Delete(cell.Id);

            return Result.Ok();
        }
    }
}