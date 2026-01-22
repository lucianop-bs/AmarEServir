using AmarEServir.Core.Results.Base;
using MediatR;

namespace Auth.API.Application.Auth.Login
{
    public record class LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;
    public record class LoginResponse(Guid id, string token, int time);

}
