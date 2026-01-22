using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.GetUserByGuid
{
    public interface IGetUserByGuidQueryHandler : IRequestHandler<GetUserByGuidQuery, Result<UserResponse>>
    {
    }

    public class GetUserByGuidQueryHandler : IGetUserByGuidQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUserByGuidQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByGuidQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByGuid(request.Id);

            if (user is null)
            {
                return Result<UserResponse>.Fail(UserErrors.Account.NotFound);
            }
            return Result<UserResponse>.Ok(user.ToResponse());
        }
    }
}