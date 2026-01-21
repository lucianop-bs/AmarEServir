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
    
    public Address Update(Address newValues)
    {
        return this with
        {
            Rua = !string.IsNullOrWhiteSpace(newValues.Rua) ? newValues.Rua : this.Rua,
            Quadra = !string.IsNullOrWhiteSpace(newValues.Quadra) ? newValues.Quadra : this.Quadra,
            Numero = !string.IsNullOrWhiteSpace(newValues.Numero) ? newValues.Numero : this.Numero,
            Bairro = !string.IsNullOrWhiteSpace(newValues.Bairro) ? newValues.Bairro : this.Bairro,
            Cidade = !string.IsNullOrWhiteSpace(newValues.Cidade) ? newValues.Cidade : this.Cidade,
            Estado = !string.IsNullOrWhiteSpace(newValues.Estado) ? newValues.Estado : this.Estado,
            Complemento = !string.IsNullOrWhiteSpace(newValues.Complemento) ? newValues.Complemento : this.Complemento,
            Cep = !string.IsNullOrWhiteSpace(newValues.Cep) ? newValues.Cep : this.Cep
        };
    }
}