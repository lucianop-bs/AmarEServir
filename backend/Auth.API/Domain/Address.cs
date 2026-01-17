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

            () => !string.IsNullOrWhiteSpace(Cep) && Cep.Length != 8
            ? Result.Fail(UserError.CepFormat)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Estado) || Estado.Length != 2
            ? Result.Fail(UserError.EstadoInvalid)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Rua) || Rua.Length > 100 || Rua.Length < 2
            ? Result.Fail(UserError.RuaInvalid)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Numero)
            ? Result.Fail(UserError.NumeroRequired)
            : Numero.Length > 20
            ? Result.Fail(UserError.NumeroLimit)
            : Result.Ok(),

            () => string.IsNullOrWhiteSpace(Bairro)
            ? Result.Fail(UserError.BairroRequired)
            : Result.Ok(),

            () => !string.IsNullOrWhiteSpace(Bairro) && (Bairro.Length < 2 || Bairro.Length > 200)
            ? Result.Fail(UserError.BairroInvalid)
            : Result.Ok(),
            () => string.IsNullOrWhiteSpace(Cidade) || Cidade.Length > 100 || Cidade.Length < 2
            ? Result.Fail(UserError.CidadeInvalid)
            : Result.Ok()
            );

        if (!resultValidation.IsSuccess)
            return Result.Fail(resultValidation.Errors);

        return Result.Ok();
    }
}