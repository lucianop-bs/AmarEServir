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

            if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != cell.Name)
            {
                if (await _cellRepository.NameExistsForAnotherCell(request.Name, request.Id))
                    return Result.Fail(CellError.NameAlreadyExists);
            }

            var leaderIdRequest = request.LeaderId ?? cell.LeaderId;

            var user = await _userRepository.GetUserByGuid(request.LeaderId);

            if (user is null)
            {
                return Result.Fail(UserErrors.Account.NotFound);
            }

            if (request.LeaderId.HasValue && request.LeaderId != cell.LeaderId)
            {

                if (await _cellRepository.LeaderExistsForAnotherCell(request.LeaderId, cell.Id))
                    return Result.Fail(CellError.AlreadyLeadingCell);
            }


            var updateResult = cell.Update(
                request.Name ?? cell.Name,
                request.LeaderId ?? cell.LeaderId,
                user
            );

            if (!updateResult.IsSuccess) return updateResult;

            await _cellRepository.Update(cell);
            return Result.Ok();

        }
    }
}
