using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Dtos;
using MediatR;

namespace Auth.API.Application.Users.UpdateUser
{
    public record class UpdateUserCommand(Guid Id, UpdateUserRequestDto User) : IRequest<Result>;
}
