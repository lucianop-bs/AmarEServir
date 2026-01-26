using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using MediatR;

namespace Auth.API.Application.Auth.Queries.Me
{
    public record class GetMeUserGuidQuery : IRequest<Result<UserResponse>>;
}