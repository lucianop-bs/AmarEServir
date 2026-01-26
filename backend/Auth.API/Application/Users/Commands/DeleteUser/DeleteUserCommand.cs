using AmarEServir.Core.Results.Base;
using MediatR;

namespace Auth.API.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : IRequest<Result>;
}