using Auth.API.Application.Users.Common.Validators;
using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {

        RuleFor(x => x.User.Name)
            .NotEmpty().WithErrorCode(UserErrors.Profile.NameRequired.Code)
            .WithMessage(UserErrors.Profile.NameRequired.Message)
            .Length(3, 50).WithErrorCode(UserErrors.Profile.NameLength.Code)
            .WithMessage(UserErrors.Profile.NameLength.Message);

        RuleFor(x => x.User.Phone)
            .NotEmpty().WithErrorCode(UserErrors.Profile.PhoneRequired.Code)
            .WithMessage(UserErrors.Profile.PhoneRequired.Message)
            .Length(11, 13).WithErrorCode(UserErrors.Profile.PhoneInvalid.Code)
            .WithMessage(UserErrors.Profile.PhoneInvalid.Message);

        RuleFor(x => x.User.Email)
            .NotEmpty().WithErrorCode(UserErrors.Account.EmailRequired.Code)
            .WithMessage(UserErrors.Account.EmailRequired.Message)
            .EmailAddress().WithErrorCode(UserErrors.Account.InvalidEmail.Code)
            .WithMessage(UserErrors.Account.InvalidEmail.Message);

        RuleFor(x => x.User.Password)

            .NotEmpty().WithErrorCode(UserErrors.Account.PasswordRequired.Code)
            .WithMessage(UserErrors.Account.PasswordRequired.Message)
            .MinimumLength(6).WithErrorCode(UserErrors.Account.WeakPassword.Code)
            .WithMessage(UserErrors.Account.WeakPassword.Message);

        RuleFor(x => x.User.Role)
            .IsInEnum().WithErrorCode(UserErrors.Account.RoleInvalid.Code)
            .WithMessage(UserErrors.Account.RoleInvalid.Message);

        RuleFor(x => x.User.Address)
            .NotNull().WithErrorCode(UserErrors.Address.AddressRequired.Code)
            .WithMessage(UserErrors.Address.AddressRequired.Message)
            .SetValidator(new AddressValidator());
    }
}