using AmarEServir.Core.Results.Base;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
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
            if (cell is null)
            {
                return Result.Fail(CellError.NotFound);
            }
            var usuario = await _userRepository.GetUserByGuid(request.LiderId);

            if (usuario is null)
            {
                return Result.Fail(UserErrors.Account.NotFound);
            }

            cell.Update(request.Name, request.LiderId, usuario);

<<<<<<< HEAD
=======
         
>>>>>>> a208f52ad21eb976f79e0dff1d708f3347d92cd9
            var validationResult = cell.Validate();
            if (!validationResult.IsSuccess)
                return validationResult;

            await _cellRepository.Update(cell);

            return Result.Ok();

        }
    }
}
