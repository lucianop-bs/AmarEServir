using Auth.API.Application.Users.CreateUser;
using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.Common.Validators;

public class AddressValidator : AbstractValidator<AddressRequest>
{
    public AddressValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithErrorCode(UserErrors.Address.CepRequired.Code).WithMessage(UserErrors.Address.CepRequired.Message)
            .Matches(@"^\d{8}$").WithErrorCode(UserErrors.Address.CepFormat.Code).WithMessage(UserErrors.Address.CepFormat.Message);

        RuleFor(x => x.Estado)
            .NotEmpty().WithErrorCode(UserErrors.Address.EstadoRequired.Code).WithMessage(UserErrors.Address.EstadoRequired.Message)
            .Length(2).WithErrorCode(UserErrors.Address.EstadoInvalid.Code).WithMessage(UserErrors.Address.EstadoInvalid.Message);

        RuleFor(x => x.Rua)
            .NotEmpty().WithErrorCode(UserErrors.Address.RuaRequired.Code).WithMessage(UserErrors.Address.RuaRequired.Message)
            .Length(2, 100).WithErrorCode(UserErrors.Address.RuaInvalid.Code).WithMessage(UserErrors.Address.RuaInvalid.Message);

        RuleFor(x => x.Bairro)
            .NotEmpty().WithErrorCode(UserErrors.Address.BairroRequired.Code).WithMessage(UserErrors.Address.BairroRequired.Message)
            .Length(2, 200).WithErrorCode(UserErrors.Address.BairroInvalid.Code).WithMessage(UserErrors.Address.BairroInvalid.Message);

        RuleFor(x => x.Cidade)
            .NotEmpty().WithErrorCode(UserErrors.Address.CidadeRequired.Code).WithMessage(UserErrors.Address.CidadeRequired.Message)
            .Length(2, 100).WithErrorCode(UserErrors.Address.CidadeInvalid.Code).WithMessage(UserErrors.Address.CidadeInvalid.Message);

        RuleFor(x => x.Numero)
            .NotEmpty().WithErrorCode(UserErrors.Address.NumeroRequired.Code).WithMessage(UserErrors.Address.NumeroRequired.Message)
            .Length(1, 20).WithErrorCode(UserErrors.Address.NumeroLimit.Code).WithMessage(UserErrors.Address.NumeroLimit.Message);
    }
}