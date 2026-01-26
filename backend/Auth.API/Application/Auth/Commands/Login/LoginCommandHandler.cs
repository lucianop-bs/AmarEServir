using AmarEServir.Core.Results.Base;
using Auth.API.Api.Configuration;
using Auth.API.Application.Services;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;
using Microsoft.Extensions.Options;

namespace Auth.API.Application.Auth.Commands.Login
{
    public interface ILoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
    }

    public class LoginCommandHandler : ILoginCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(command.Request.Email);

            if (user == null)
            {
                return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(command.Request.Password, user.Password);

            if (!isPasswordValid)
            {
                return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);
            }

            var accessToken = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.AddRefreshToken(refreshToken, daysToExpire: 7);
            await _userRepository.Update(user);

            var response = new LoginResponse(
                user.Id,
                accessToken,
                refreshToken,
                _jwtSettings.ExpirationInMinutes * 60
            );

            return Result<LoginResponse>.Ok(response);
        }
    }
}