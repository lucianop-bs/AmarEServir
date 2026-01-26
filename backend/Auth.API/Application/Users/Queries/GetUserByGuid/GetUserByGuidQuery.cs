using AmarEServir.Core.Results.Base;
using Auth.API.Application.Common.Models;
using MediatR;

namespace Auth.API.Application.Users.Queries.GetUserByGuid;

public record class GetUserByGuidQuery(Guid Id) : IRequest<Result<UserResponse>>;