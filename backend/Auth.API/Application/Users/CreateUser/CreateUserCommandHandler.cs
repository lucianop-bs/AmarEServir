using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Dtos;
using Auth.API.Application.Users.Mappers;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{

    public interface ICreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponseDto>>
    {
    }
    public class CreateUserCommandHandler : ICreateUserCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ICellRepository _cellRepository;

        public CreateUserCommandHandler(IUserRepository userRepository, ICellRepository cellRepository)
        {
            _userRepository = userRepository;

            _cellRepository = cellRepository;
        }
        public async Task<Result<UserResponseDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userEmailAlreadyExists = await _userRepository.GetUserByEmail(request.User.Email);

            if (userEmailAlreadyExists is not null)
            {
                return Result<UserResponseDto>.Fail(UserError.Account.EmailAlreadyExists);
            }

            var user = request.User.ToDomain();

            var userValidate = user.Validate();

            if (!userValidate.IsSuccess)
                return Result<UserResponseDto>.Fail(userValidate.Errors);

            await _userRepository.Create(user);

            var response = user.ToResponseDto();

            return Result<UserResponseDto>.Ok(response);

        }
    }
}
