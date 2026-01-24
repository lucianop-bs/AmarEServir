using AmarEServir.Core.Common;
using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using MediatR;

namespace Auth.API.Application.Users.GetUsersByQuery
{
    public record GetUsersQuery(int Page, int PageSize, string? SearchTerm)
      : IRequest<Result<PagedResult<UserResponse>>>;
}