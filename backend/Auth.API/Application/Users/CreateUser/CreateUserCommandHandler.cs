using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{

    public interface ICreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserModelView>>
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
        public async Task<Result<UserModelView>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userEmailAlreadyExists = await _userRepository.GetUserByEmail(request.Email);

            if (userEmailAlreadyExists is not null)
            {
                return Result<UserModelView>.Fail(UserErrors.Account.EmailAlreadyExists);
            }

            var user = request.ToDomain();

            var userValidate = user.Validate();

            if (!userValidate.IsSuccess)
                return Result<UserModelView>.Fail(userValidate.Errors);

            await _userRepository.Create(user);

            var response = user.ToModelUserView();

            return Result<UserModelView>.Ok(response);

        }
    }
}
