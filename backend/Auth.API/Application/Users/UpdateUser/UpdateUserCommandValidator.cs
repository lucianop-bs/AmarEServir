using Auth.API.Application.Users.Dtos;
using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.UpdateUser;

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .Length(3, 50).WithErrorCode(UserError.Profile.NameLength.Code)
            .WithMessage(UserError.Profile.NameLength.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Email)
            .EmailAddress().WithErrorCode(UserError.Account.InvalidEmail.Code)
            .WithMessage(UserError.Account.InvalidEmail.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .Length(11, 13).WithErrorCode(UserError.Profile.PhoneInvalid.Code)
            .WithMessage(UserError.Profile.PhoneInvalid.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Role)
            .IsInEnum().WithErrorCode(UserError.Account.RoleInvalid.Code)
            .WithMessage(UserError.Account.RoleInvalid.Message)
            .When(x => x.Role.HasValue);

        RuleFor(x => x.Address)
            .SetValidator(new AddressRequestDtoValidator())
            .When(x => x.Address != null);
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode(UserError.Account.IdRequired.Code)
            .WithMessage(UserError.Account.IdRequired.Message);

        RuleFor(x => x.User)
            .NotNull().WithMessage(UserError.Account.UpdateAccountRequired.Message)
            .WithErrorCode(UserError.Account.UpdateAccountRequired.Code)
            .SetValidator(new UpdateUserRequestDtoValidator());
    }
}

public class AddressRequestDtoValidator : AbstractValidator<AddressRequestDto>
{
    public AddressRequestDtoValidator()
    {
        RuleFor(x => x.Rua)
            .NotEmpty().WithErrorCode(UserError.Address.RuaRequired.Code)
            .WithMessage(UserError.Address.RuaRequired.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Rua));

        RuleFor(x => x.Numero)
            .NotEmpty().WithErrorCode(UserError.Address.NumeroRequired.Code)
            .WithMessage(UserError.Address.NumeroRequired.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Numero));

        RuleFor(x => x.Bairro)
            .NotEmpty().WithErrorCode(UserError.Address.BairroRequired.Code)
            .WithMessage(UserError.Address.BairroRequired.Message)
             .When(x => !string.IsNullOrWhiteSpace(x.Bairro));

        RuleFor(x => x.Cidade)
            .NotEmpty().WithErrorCode(UserError.Address.CidadeRequired.Code)
            .WithMessage(UserError.Address.CidadeRequired.Message)
             .When(x => !string.IsNullOrWhiteSpace(x.Cidade));

        RuleFor(x => x.Estado)
            .NotEmpty().WithErrorCode(UserError.Address.EstadoRequired.Code)
            .WithMessage(UserError.Address.EstadoRequired.Message)
              .When(x => !string.IsNullOrWhiteSpace(x.Estado));

        RuleFor(x => x.Cep)
            .NotEmpty().WithErrorCode(UserError.Address.CepRequired.Code)
            .WithMessage(UserError.Address.CepRequired.Message)
            .When(x => !string.IsNullOrWhiteSpace(x.Cep));
    }
}