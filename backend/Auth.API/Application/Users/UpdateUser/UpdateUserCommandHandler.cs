using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Mappers;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.UpdateUser
{

    public interface IUpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    { }
    public class UpdateUserCommandHandler : IUpdateUserCommandHandler
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
                return Result.Fail(UserError.Account.NotFound);

            var userEmailAlreadyExists = await _userRepository.EmailExistsForAnotherUser(request.User.Email,request.Id);

            if (userEmailAlreadyExists)
            {
                return Result.Fail(UserError.Account.EmailAlreadyExists);
            }

            var addressUpdate = request.User.Address != null ? request.User.Address.ToDomain() : null;

            user.UserUpdate(
                request.User.Name,
                request.User.Email,
                request.User.Phone,
                addressUpdate,
                request.User.Role);
           

            var validationUser = user.Validate();
            if (!validationUser.IsSuccess)
                return validationUser;

            await _userRepository.Update(user);

            return Result.Ok();
        }
    }
}
