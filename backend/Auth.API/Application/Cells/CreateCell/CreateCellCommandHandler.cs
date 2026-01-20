using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Models;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
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
                return Result<CellModelView>.Fail(UserErrors.Account.NotFound);
            }

            if (await _cellRepository.LeaderExists(request.LeaderId))
            {
                return Result<CellModelView>.Fail(CellError.AlreadyLeadingCell);
            }

            if (await _cellRepository.NameAlreadyExist(request.Name))
            {
                return Result<CellModelView>.Fail(CellError.NameAlreadyExists);
            }

            var cellValidation = Cell.Create(request.Name, user);

            if (!cellValidation.IsSuccess)
            {
                return Result<CellModelView>.Fail(cellValidation.Errors);
            }

            var cell = cellValidation.Value;

            await _cellRepository.Create(cell);

            var response = cell.ToModelView();

            return Result<CellModelView>.Ok(response);
        }
    }
}
