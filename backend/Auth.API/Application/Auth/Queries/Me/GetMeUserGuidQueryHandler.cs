using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using Auth.API.Application.Services;
using Auth.API.Application.Users.Commands.CreateUser;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Auth.Queries.Me
{
    public interface IGetMeUserGuidQueryHandler : IRequestHandler<GetMeUserGuidQuery, Result<UserResponse>>
    {
    }

    public class GetMeUserGuidQueryHandler : IGetMeUserGuidQueryHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMeUserGuidQueryHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result<UserResponse>?> Handle(GetMeUserGuidQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserService.UserId;

            if (currentUser is null)
            {
                return Result<UserResponse>.Fail(UserErrors.Account.NotFound);
            }

            var user = await _userRepository.GetUserByGuid(currentUser);

            if (user is null)
            {
                return Result<UserResponse>.Fail(UserErrors.Account.NotFound);
            }

            var response = user.ToUserResponse();

            return Result<UserResponse>.Ok(response);
        }
    }
}