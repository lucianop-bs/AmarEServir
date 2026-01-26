using AmarEServir.Core.Results.Base;
using MediatR;

namespace Auth.API.Application.Auth.Commands.Login
{
    public record class LoginCommand(
        LoginRequest Request) : IRequest<Result<LoginResponse>>;
    public record class LoginResponse(
        Guid Id,
        string Token,
        string RefreshToken,
        int Time);

    public record class LoginRequest(
         string Email,
         string Password);
}