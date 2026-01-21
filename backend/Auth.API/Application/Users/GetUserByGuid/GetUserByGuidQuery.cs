using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using MediatR;

namespace Auth.API.Application.Users.GetUserByGuid;

public record class GetUserByGuidQuery(Guid Id) : IRequest<Result<UserResponse>>;

