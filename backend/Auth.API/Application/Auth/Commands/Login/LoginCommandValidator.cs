using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Request.Email)
              .NotEmpty()
              .WithMessage(AuthErrors.EmailRequired.Message)
              .WithErrorCode(AuthErrors.EmailRequired.Code);

            RuleFor(x => x.Request.Email)
             .EmailAddress()
             .WithMessage(AuthErrors.InvalidEmail.Message)
             .WithErrorCode(AuthErrors.InvalidEmail.Code)
             .When(x => !string.IsNullOrEmpty(x.Request.Email)); ;

            RuleFor(x => x.Request.Password)
             .NotEmpty()
             .WithMessage(AuthErrors.PasswordRequired.Message)
             .WithErrorCode(AuthErrors.PasswordRequired.Code)
             .MinimumLength(6)
             .WithMessage(AuthErrors.WeakPassword.Message)
             .WithErrorCode(AuthErrors.WeakPassword.Code);
        }
    }
}