using AmarEServir.Core.Results.Base;
using Auth.API.Application.Auth.Commands.Login;
using MediatR;

namespace Auth.API.Application.Auth.Commands.Refresh
{
    public record class RefreshTokenCommand(string RefreshToken) : IRequest<Result<LoginResponse>>;
}