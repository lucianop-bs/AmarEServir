using AmarEServir.Core.Results.Base;
using Auth.API.Application.Cells.Dtos;
using Auth.API.Application.Cells.Mappers;
using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Cells.CreateCell
{

    public class CreateCellCommandHandler : IRequestHandler<CreateCellCommand, Result<CellResponseDto>>
    {
        private readonly ICellRepository _cellRepository;
        private readonly IUserRepository _userRepository;

        public CreateCellCommandHandler(ICellRepository cellRepository, IUserRepository userRepository)
        {
            _cellRepository = cellRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<CellResponseDto>> Handle(CreateCellCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetUserByGuid(request.Cell.LeaderId);

            if (user is null)
            {
                return Result<CellResponseDto>.Fail(UserError.Account.NotFound);
            }

            if (await _cellRepository.LeaderExists(request.Cell.LeaderId))
            {
                return Result<CellResponseDto>.Fail(CellError.AlreadyLeadingCell);
            }

            if (await _cellRepository.NameAlreadyExists(request.Cell.Name))
            {
                return Result<CellResponseDto>.Fail(CellError.NameAlreadyExists);
            }

            var cellValidation = Cell.Create(request.Cell.Name, user);

            if (!cellValidation.IsSuccess)
            {
                return Result<CellResponseDto>.Fail(cellValidation.Errors);
            }

            var cell = cellValidation.Value;

            await _cellRepository.Create(cell);

            var response = cell.ToResponseDto();

            return Result<CellResponseDto>.Ok(response);
        }
    }
}
