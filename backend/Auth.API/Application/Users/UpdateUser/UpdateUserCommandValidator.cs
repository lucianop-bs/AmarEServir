using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.UpdateUser;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .Length(3, 50).WithErrorCode(UserErrors.Profile.NameLength.Code)
            .WithMessage(UserErrors.Profile.NameLength.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Email)
            .EmailAddress().WithErrorCode(UserErrors.Account.InvalidEmail.Code)
            .WithMessage(UserErrors.Account.InvalidEmail.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .Length(11, 13).WithErrorCode(UserErrors.Profile.PhoneInvalid.Code)
            .WithMessage(UserErrors.Profile.PhoneInvalid.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Role)
            .IsInEnum().WithErrorCode(UserErrors.Account.RoleInvalid.Code)
            .WithMessage(UserErrors.Account.RoleInvalid.Message)
            .When(x => x.Role.HasValue);

        RuleFor(x => x.Address)
            .SetValidator(new UpdateAddressValidator())
            .When(x => x.Address != null);
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode(UserErrors.Account.IdRequired.Code)
            .WithMessage(UserErrors.Account.IdRequired.Message);

        RuleFor(x => x.User)
            .NotNull().WithMessage(UserErrors.Account.UpdateAccountRequired.Message)
            .WithErrorCode(UserErrors.Account.UpdateAccountRequired.Code)
            .SetValidator(new UpdateUserRequestValidator());
    }
}

public class UpdateAddressValidator : AbstractValidator<AddressRequest>
{
    public UpdateAddressValidator()
    {
        RuleFor(x => x.Cep)
            .Matches(@"^\d{8}$")
            .WithErrorCode(UserErrors.Address.CepFormat.Code)
            .WithMessage(UserErrors.Address.CepFormat.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Cep));

        RuleFor(x => x.Estado)
            .Length(2)
            .WithErrorCode(UserErrors.Address.EstadoInvalid.Code)
            .WithMessage(UserErrors.Address.EstadoInvalid.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Estado));

        RuleFor(x => x.Rua)
            .Length(2, 100)
            .WithErrorCode(UserErrors.Address.RuaInvalid.Code)
            .WithMessage(UserErrors.Address.RuaInvalid.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Rua));

        RuleFor(x => x.Bairro)
            .Length(2, 200)
            .WithErrorCode(UserErrors.Address.BairroInvalid.Code)
            .WithMessage(UserErrors.Address.BairroInvalid.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Bairro));

        RuleFor(x => x.Cidade)
            .Length(2, 100)
            .WithErrorCode(UserErrors.Address.CidadeInvalid.Code)
            .WithMessage(UserErrors.Address.CidadeInvalid.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Cidade));

        RuleFor(x => x.Numero)
            .Length(1, 20)
            .WithErrorCode(UserErrors.Address.NumeroLimit.Code)
            .WithMessage(UserErrors.Address.NumeroLimit.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Numero));
    }
}
