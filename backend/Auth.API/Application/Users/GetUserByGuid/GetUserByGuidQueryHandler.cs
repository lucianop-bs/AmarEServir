using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.GetUserByGuid
{

    public interface IGetUserByGuidQueryHandler : IRequestHandler<GetUserByGuidQuery, Result<UserModelView>>
    {
    }
    public class GetUserByGuidQueryHandler : IGetUserByGuidQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUserByGuidQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserModelView>> Handle(GetUserByGuidQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByGuid(request.Id);

            if (user is null)
            {
                return Result<UserModelView>.Fail(UserErrors.Account.NotFound);

            }
            return Result<UserModelView>.Ok(user.ToModelUserView());

        }
    }
}
