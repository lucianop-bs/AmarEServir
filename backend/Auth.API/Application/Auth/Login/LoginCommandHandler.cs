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

        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            // ✅ CORREÇÃO 1: Usamos .Request porque seu Command tem esse wrapper
            var user = await _userRepository.GetUserByEmail(command.Request.Email);

            if (user == null)
            {
                return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);
            }

            // ✅ CORREÇÃO 2: Acessamos Password via .Request
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(command.Request.Password, user.Password);

            if (!isPasswordValid)
            {
                return Result<LoginResponse>.Fail(AuthErrors.InvalidCredentials);
            }

            // 3. Gerar Tokens
            var accessToken = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // 4. Salvar Refresh Token (Lembre-se de ter atualizado o User.cs e MongoDbMapping.cs!)
            user.AddRefreshToken(refreshToken, daysToExpire: 7);
            await _userRepository.Update(user);

            // ✅ CORREÇÃO 3: Mapeamento para o seu LoginResponse (Guid, Token, RefreshToken, Time)
            // Ordem do record: Id, Token, RefreshToken, Time
            var response = new LoginResponse(
                user.Id,
                accessToken,
                refreshToken,
                _jwtSettings.ExpirationInMinutes * 60 // Converte para segundos (int)
            );

            return Result<LoginResponse>.Ok(response);
        }
    }
}