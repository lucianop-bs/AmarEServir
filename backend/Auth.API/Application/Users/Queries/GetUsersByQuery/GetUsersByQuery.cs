using AmarEServir.Core.Common;
using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using MediatR;

namespace Auth.API.Application.Users.Queries.GetUsersByQuery
{
    public record GetUsersQuery(int Page, int PageSize, string? SearchTerm)
      : IRequest<Result<PagedResult<UserResponse>>>;
}