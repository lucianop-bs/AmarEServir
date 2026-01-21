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
            .SetValidator(new AddressRequestValidator())
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

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(x => x.Rua)
            .NotEmpty().WithErrorCode(UserErrors.Address.RuaRequired.Code)
            .WithMessage(UserErrors.Address.RuaRequired.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Rua));

        RuleFor(x => x.Numero)
            .NotEmpty().WithErrorCode(UserErrors.Address.NumeroRequired.Code)
            .WithMessage(UserErrors.Address.NumeroRequired.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Numero));

        RuleFor(x => x.Bairro)
            .NotEmpty().WithErrorCode(UserErrors.Address.BairroRequired.Code)
            .WithMessage(UserErrors.Address.BairroRequired.Message)
             .When(x => !string.IsNullOrWhiteSpace(x.Bairro));

        RuleFor(x => x.Cidade)
            .NotEmpty().WithErrorCode(UserErrors.Address.CidadeRequired.Code)
            .WithMessage(UserErrors.Address.CidadeRequired.Message)
             .When(x => !string.IsNullOrWhiteSpace(x.Cidade));

        RuleFor(x => x.Estado)
            .NotEmpty().WithErrorCode(UserErrors.Address.EstadoRequired.Code)
            .WithMessage(UserErrors.Address.EstadoRequired.Message)
              .When(x => !string.IsNullOrWhiteSpace(x.Estado));

        RuleFor(x => x.Cep)
            .NotEmpty().WithErrorCode(UserErrors.Address.CepRequired.Code)
            .WithMessage(UserErrors.Address.CepRequired.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Cep));
    }
}