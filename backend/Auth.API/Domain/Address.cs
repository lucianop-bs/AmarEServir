using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;
using Auth.API.Domain.Errors;

namespace Auth.API.Domain;

public record class Address(
    string Rua,
    string Quadra,
    string Numero,
    string Bairro,
    string Cidade,
    string Estado,
    string Complemento,
    string Cep)
{
    public Result Validate()
    {
        var resultValidation = ResultValidation.ValidateCollectErrors(

            () => string.IsNullOrWhiteSpace(Cep)
                ? Result.Fail(UserErrors.Address.CepRequired)
                : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Estado)
                ? Result.Fail(UserErrors.Address.EstadoRequired)
                : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Rua)
                ? Result.Fail(UserErrors.Address.RuaRequired)
                : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Numero)
                ? Result.Fail(UserErrors.Address.NumeroRequired)
                : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Bairro)
                ? Result.Fail(UserErrors.Address.BairroRequired)
                : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Cidade)
                ? Result.Fail(UserErrors.Address.CidadeRequired)
                : Result.Ok()
        );

        if (!resultValidation.IsSuccess)
            return Result.Fail(resultValidation.Errors);

        return Result.Ok();
    }
}