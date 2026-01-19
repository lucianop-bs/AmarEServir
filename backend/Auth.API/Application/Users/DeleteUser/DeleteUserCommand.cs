using AmarEServir.Core.Results.Base;
using MediatR;

namespace Auth.API.Application.Users.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : IRequest<Result>;

}
