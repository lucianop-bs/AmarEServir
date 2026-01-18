using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Models;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using MediatR;

namespace Auth.API.Application.Cells.CreateCell
{

    public class CreateCellCommandHandler : IRequestHandler<CreateCellCommand, Result<CellModelView>>
    {
        private readonly ICellRepository _cellRepository;
        private readonly IUserRepository _userRepository;

        public CreateCellCommandHandler(ICellRepository cellRepository, IUserRepository userRepository)
        {
            _cellRepository = cellRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<CellModelView>> Handle(CreateCellCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetUserByGuid(request.LeaderId);

            if (user is null)
            {
                return Result<CellModelView>.Fail(UserError.NotFound);
            }

            var userLeaderValidation = IsLeader(user, request.LeaderId);

            if (!userLeaderValidation.IsSuccess)
            {

                return Result<CellModelView>.Fail(CellError.LeaderRequired);

            }

            var cell = new Cell(request.Name, user.Id, user);
            var cellValidation = cell.Validate();

            if (!cellValidation.IsSuccess)
            {
                return Result<CellModelView>.Fail(cellValidation.Errors);
            }

            await _cellRepository.Create(cell);

            var response = cell.ToModelView();

            return Result<CellModelView>.Ok(response);
        }

        public Result IsLeader(User user)
        {
            if (user.Role != UserRole.Leader)
            {
                return Result.Fail(CellError.LeaderRequired);
            }

            return Result.Ok();
        }
    }
}
