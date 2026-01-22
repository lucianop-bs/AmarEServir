using AmarEServir.Core.Results.Base;
using Auth.API.Application.Auth.Login;
using MediatR;

namespace Auth.API.Application.Auth.Refresh
{
    public record class RefreshTokenCommand(string RefreshToken) : IRequest<Result<LoginResponse>>;
}