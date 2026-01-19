using AmarEServir.Core.Results.Base;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.UpdateUser
{

    public interface IUpdateUserComandHandler : IRequestHandler<UpdateUserCommand, Result>
    { }
    public class UpdateUserCommandHandler : IUpdateUserComandHandler
    {

        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByGuid(request.Id);

            if (user is null)
                return Result.Fail(UserErrors.Account.NotFound);

            user.UserUpdate(
                request.User.Name,
                request.User.Email,
                request.User.Phone,
                request.User.Password,
                request.User.Address,
                request.User.Role);

            var validationUser = user.Validate();
            if (!validationUser.IsSuccess)
                return validationUser;

            await _userRepository.Update(user);

            return Result.Ok();
        }
    }
}
