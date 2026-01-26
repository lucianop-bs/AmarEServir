using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Cells.Commands.CreateCell
{
    public class CreateCellCommandHandler : IRequestHandler<CreateCellCommand, Result<CellResponse>>
    {
        private readonly ICellRepository _cellRepository;
        private readonly IUserRepository _userRepository;

        public CreateCellCommandHandler(ICellRepository cellRepository, IUserRepository userRepository)
        {
            _cellRepository = cellRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<CellResponse>> Handle(CreateCellCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByGuid(request.Cell.LeaderId);

            if (user is null)
            {
                return Result<CellResponse>.Fail(UserErrors.Account.NotFound);
            }

            if (await _cellRepository.LeaderExists(request.Cell.LeaderId))
            {
                return Result<CellResponse>.Fail(CellError.AlreadyLeadingCell);
            }

            if (string.IsNullOrWhiteSpace(request.Cell.Name))
            {
                return Result<CellResponse>.Fail(CellError.NameRequired);
            }

            if (await _cellRepository.NameAlreadyExists(request.Cell.Name))
            {
                return Result<CellResponse>.Fail(CellError.NameAlreadyExists);
            }

            var cellValidation = Cell.Create(request.Cell.Name, user);

            if (!cellValidation.IsSuccess)
            {
                return Result<CellResponse>.Fail(cellValidation.Errors);
            }

            var cell = cellValidation.Value;

            await _cellRepository.Create(cell);

            var response = cell.ToResponse();

            return Result<CellResponse>.Ok(response);
        }
    }
}