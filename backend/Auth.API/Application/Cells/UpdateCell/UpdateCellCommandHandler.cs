using AmarEServir.Core.Results.Base;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using MediatR;

namespace Auth.API.Application.Cells.UpdateCell
{

    public interface IUpdateCellCommandHandler
    : IRequestHandler<UpdateCellCommand, Result>
    { }
    public class UpdateCellCommandHandler : IUpdateCellCommandHandler
    {
        private readonly ICellRepository _cellRepository;
        private readonly IUserRepository _userRepository;
        public UpdateCellCommandHandler(ICellRepository cellRepository, IUserRepository userRepository)
        {
            _cellRepository = cellRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(UpdateCellCommand request, CancellationToken cancellationToken)
        {
            var cell = await _cellRepository.GetCellByGuid(request.Id);
            var usuario = await _userRepository.GetUserByGuid(request.LiderId);

            if (usuario is null)
            {
                return Result.Fail(UserError.NotFound);
            }
            if (cell is null)
            {
                return Result.Fail(CellError.NotFound);
            }

            var cellUpdate = cell.Update(request.Name, request.LiderId, usuario);

            
            if (!cellUpdate.IsSuccess)
            {
                return Result.Fail(cellUpdate.Errors);
            }
            await _cellRepository.Update(cellUpdate.Value);
            return Result.Ok();

        }
    }
}
