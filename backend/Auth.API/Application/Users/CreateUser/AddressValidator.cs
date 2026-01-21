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
            .NotEmpty().WithErrorCode(UserError.Address.CepRequired.Code).WithMessage(UserError.Address.CepRequired.Message)
            .Matches(@"^\d{8}$").WithErrorCode(UserError.Address.CepFormat.Code).WithMessage(UserError.Address.CepFormat.Message);

        // Estado
        RuleFor(x => x.Estado)
            .NotEmpty().WithErrorCode(UserError.Address.EstadoRequired.Code).WithMessage(UserError.Address.EstadoRequired.Message)
            .Length(2).WithErrorCode(UserError.Address.EstadoInvalid.Code).WithMessage(UserError.Address.EstadoInvalid.Message);

        // Rua
        RuleFor(x => x.Rua)
            .NotEmpty().WithErrorCode(UserError.Address.RuaRequired.Code).WithMessage(UserError.Address.RuaRequired.Message)
            .Length(2, 100).WithErrorCode(UserError.Address.RuaInvalid.Code).WithMessage(UserError.Address.RuaInvalid.Message);

        // Bairro
        RuleFor(x => x.Bairro)
            .NotEmpty().WithErrorCode(UserError.Address.BairroRequired.Code).WithMessage(UserError.Address.BairroRequired.Message)
            .Length(2, 200).WithErrorCode(UserError.Address.BairroInvalid.Code).WithMessage(UserError.Address.BairroInvalid.Message);

        // Cidade
        RuleFor(x => x.Cidade)
            .NotEmpty().WithErrorCode(UserError.Address.CidadeRequired.Code).WithMessage(UserError.Address.CidadeRequired.Message)
            .Length(2, 100).WithErrorCode(UserError.Address.CidadeInvalid.Code).WithMessage(UserError.Address.CidadeInvalid.Message);

        // Número
        RuleFor(x => x.Numero)
            .NotEmpty().WithErrorCode(UserError.Address.NumeroRequired.Code).WithMessage(UserError.Address.NumeroRequired.Message)
            .Length(1, 20).WithErrorCode(UserError.Address.NumeroLimit.Code).WithMessage(UserError.Address.NumeroLimit.Message);
    }
}