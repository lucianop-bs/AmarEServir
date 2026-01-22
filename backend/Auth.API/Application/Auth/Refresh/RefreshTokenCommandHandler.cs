using AmarEServir.Core.Results.Base;
using Auth.API.Api.Configurations;
using Auth.API.Application.Auth.Login;
using Auth.API.Application.Services;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Errors;
using MediatR;
using Microsoft.Extensions.Options;

namespace Auth.API.Application.Auth.Refresh
{
    public interface IRefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
    {
    }

    public class RefreshTokenCommandHandler : IRefreshTokenCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(request.RefreshToken);

            if (user == null)
            {
                return Result<LoginResponse>.Fail(AuthErrors.TokenRefreshRequired);
            }

            var isSuccess = user.UseRefreshToken(request.RefreshToken);

            if (!isSuccess)
            {
                await _userRepository.Update(user);
                return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);
            }

            var newAccessToken = _jwtTokenService.GenerateToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            user.AddRefreshToken(newRefreshToken);

            await _userRepository.Update(user);

            var response = new LoginResponse(
                user.Id,
                newAccessToken,
                newRefreshToken,
                _jwtSettings.ExpirationInMinutes * 60
            );

            return Result<LoginResponse>.Ok(response);
        }
    }
}