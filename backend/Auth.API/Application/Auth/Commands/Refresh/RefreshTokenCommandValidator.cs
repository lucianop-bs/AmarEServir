using FluentValidation;

namespace Auth.API.Application.Auth.Commands.Refresh
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("O Refresh Token é obrigatório.")
                .MinimumLength(32).WithMessage("Refresh Token inválido (formato incorreto).");
        }
    }
}