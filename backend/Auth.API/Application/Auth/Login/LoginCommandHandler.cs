using AmarEServir.Core.Results.Base;
using Auth.API.Api.Configurations;
using Auth.API.Application.Services;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;
using Microsoft.Extensions.Options;

namespace Auth.API.Application.Auth.Login
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

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!isPasswordValid)
            {
                return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);
            }
            var token = _jwtTokenService.GenerateToken(user);

            var response = new LoginResponse(user.Id, token, _jwtSettings.ExpirationInMinutes * 60);

            return Result<LoginResponse>.Ok(response);
        }
    }
}