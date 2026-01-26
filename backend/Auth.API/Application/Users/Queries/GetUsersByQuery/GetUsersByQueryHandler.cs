using AmarEServir.Core.Common;
using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using Auth.API.Application.Users.Commands.CreateUser;
using Auth.API.Domain.Contracts;
using MediatR;

namespace Auth.API.Application.Users.Queries.GetUsersByQuery
{
    public interface IGetUsersByQueryHandler : IRequestHandler<GetUsersQuery, Result<PagedResult<UserResponse>>>
    {
    }

    public class GetUsersByQueryHandler : IGetUsersByQueryHandler
    {
        private readonly IUserRepository userRepository;

        public GetUsersByQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<PagedResult<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var usersPaged = await userRepository.GetUsersByQueryAsync(request.Page, request.PageSize, request.SearchTerm);

            var responses = usersPaged.Items.Select(u => u.ToUserResponse()).ToList();

            var responsePaged = new PagedResult<UserResponse>(
                responses,
                usersPaged.PageNumbers,
                usersPaged.PageSize,
                usersPaged.TotalCounts
            );

            return Result<PagedResult<UserResponse>>.Ok(responsePaged);
        }
    }
}