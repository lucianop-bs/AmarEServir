using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {

        RuleFor(x => x.User.Name)
            .NotEmpty().WithErrorCode(UserError.Profile.NameRequired.Code)
            .WithMessage(UserError.Profile.NameRequired.Message)
            .Length(3, 50).WithErrorCode(UserError.Profile.NameLength.Code)
            .WithMessage(UserError.Profile.NameLength.Message);

        RuleFor(x => x.User.Phone)
            .NotEmpty().WithErrorCode(UserError.Profile.PhoneRequired.Code)
            .WithMessage(UserError.Profile.PhoneRequired.Message)
            .Length(11, 13).WithErrorCode(UserError.Profile.PhoneInvalid.Code)
            .WithMessage(UserError.Profile.PhoneInvalid.Message);

        RuleFor(x => x.User.Email)
            .NotEmpty().WithErrorCode(UserError.Account.EmailRequired.Code)
            .WithMessage(UserError.Account.EmailRequired.Message)
            .EmailAddress().WithErrorCode(UserError.Account.InvalidEmail.Code)
            .WithMessage(UserError.Account.InvalidEmail.Message);

        RuleFor(x => x.User.Password)
           
            .NotEmpty().WithErrorCode(UserError.Account.PasswordRequired.Code)
            .WithMessage(UserError.Account.PasswordRequired.Message)
            .MinimumLength(6).WithErrorCode(UserError.Account.WeakPassword.Code)
            .WithMessage(UserError.Account.WeakPassword.Message);

        RuleFor(x => x.User.Role)
            .IsInEnum().WithErrorCode(UserError.Account.RoleInvalid.Code)
            .WithMessage(UserError.Account.RoleInvalid.Message);

        RuleFor(x => x.User.Address)
            .NotNull().WithErrorCode(UserError.Address.AddressRequired.Code)
            .WithMessage(UserError.Address.AddressRequired.Message)
            .SetValidator(new AddressRequestValidator());
    }
}