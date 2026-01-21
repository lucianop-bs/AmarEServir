using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Dtos;
using Auth.API.Application.Users.Mappers;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;

namespace Auth.API.Application.Users.GetUserByGuid
{

    public interface IGetUserByGuidQueryHandler : IRequestHandler<GetUserByGuidQuery, Result<UserResponseDto>>
    {
    }
    public class GetUserByGuidQueryHandler : IGetUserByGuidQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUserByGuidQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponseDto>> Handle(GetUserByGuidQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByGuid(request.Id);

            if (user is null)
            {
                return Result<UserResponseDto>.Fail(UserError.Account.NotFound);

            }
            return Result<UserResponseDto>.Ok(user.ToResponseDto());

        }
    }
}
