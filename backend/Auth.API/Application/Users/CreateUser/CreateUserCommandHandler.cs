using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.CreateUser.Mappers;
using Auth.API.Application.Users.Models;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{

    public interface ICreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
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
        public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userEmailAlreadyExists = await _userRepository.GetUserByEmail(request.User.Email);

            if (userEmailAlreadyExists is not null)
            {
                return Result<UserResponse>.Fail(UserErrors.Account.EmailAlreadyExists);
            }
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.User.Password);
            var user = request.ToUser(hashPassword);

            var userValidate = user.Validate();

            if (!userValidate.IsSuccess)
                return Result<UserResponse>.Fail(userValidate.Errors);

            await _userRepository.Create(user);

            var response = user.ToResponse();

            return Result<UserResponse>.Ok(response);

        }
    }
}
