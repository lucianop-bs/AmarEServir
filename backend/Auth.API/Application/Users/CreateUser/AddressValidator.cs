using Auth.API.Domain;
using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.CreateUser;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        // CEP
        RuleFor(x => x.Cep)
            .NotEmpty().WithErrorCode(UserErrors.Address.CepRequired.Code).WithMessage(UserErrors.Address.CepRequired.Message)
            .Matches(@"^\d{8}$").WithErrorCode(UserErrors.Address.CepFormat.Code).WithMessage(UserErrors.Address.CepFormat.Message);

        // Estado
        RuleFor(x => x.Estado)
            .NotEmpty().WithErrorCode(UserErrors.Address.EstadoRequired.Code).WithMessage(UserErrors.Address.EstadoRequired.Message)
            .Length(2).WithErrorCode(UserErrors.Address.EstadoInvalid.Code).WithMessage(UserErrors.Address.EstadoInvalid.Message);

        // Rua
        RuleFor(x => x.Rua)
            .NotEmpty().WithErrorCode(UserErrors.Address.RuaRequired.Code).WithMessage(UserErrors.Address.RuaRequired.Message)
            .Length(2, 100).WithErrorCode(UserErrors.Address.RuaInvalid.Code).WithMessage(UserErrors.Address.RuaInvalid.Message);

        // Bairro
        RuleFor(x => x.Bairro)
            .NotEmpty().WithErrorCode(UserErrors.Address.BairroRequired.Code).WithMessage(UserErrors.Address.BairroRequired.Message)
            .Length(2, 200).WithErrorCode(UserErrors.Address.BairroInvalid.Code).WithMessage(UserErrors.Address.BairroInvalid.Message);

        // Cidade
        RuleFor(x => x.Cidade)
            .NotEmpty().WithErrorCode(UserErrors.Address.CidadeRequired.Code).WithMessage(UserErrors.Address.CidadeRequired.Message)
            .Length(2, 100).WithErrorCode(UserErrors.Address.CidadeInvalid.Code).WithMessage(UserErrors.Address.CidadeInvalid.Message);

        // Número
        RuleFor(x => x.Numero)
            .NotEmpty().WithErrorCode(UserErrors.Address.NumeroRequired.Code).WithMessage(UserErrors.Address.NumeroRequired.Message)
            .Length(1, 20).WithErrorCode(UserErrors.Address.NumeroLimit.Code).WithMessage(UserErrors.Address.NumeroLimit.Message);
    }
}