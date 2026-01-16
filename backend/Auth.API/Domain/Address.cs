using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;

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
            ? Result.Fail(UserError.CepRequired)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Estado) || Estado.Length != 2
            ? Result.Fail(UserError.EstadoInvalid)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Rua)
            ? Result.Fail(UserError.RuaInvalid)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Numero)
            ? Result.Fail(UserError.NumeroLimit)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Bairro)
            ? Result.Fail(UserError.BairroRequired)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Cidade)
            ? Result.Fail(UserError.CidadeRequired)
            : Result.Ok()
            );
        if (!resultValidation.IsSuccess)
            return Result.Fail(resultValidation.Errors);

        return Result.Ok();
    }
}