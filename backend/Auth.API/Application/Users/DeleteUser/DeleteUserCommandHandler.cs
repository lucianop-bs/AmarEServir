using AmarEServir.Core.Results.Base;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.DeleteUser
{
    public interface IDeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>;

    public class DeleteUserCommandHandler : IDeleteUserCommandHandler
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByGuid(request.Id);

            if (user is null)
            {
                return Result.Fail(UserErrors.Account.NotFound);
            }

            await _userRepository.Delete(user.Id);

            return Result.Ok();
        }
    }
}