using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Extensions;
using System.Net;

namespace AmarEServir.Core.Results.Api;

public interface IApiResult
{
    bool Success { get; }
    int Status { get; }
    IReadOnlyList<ApiError> Errors { get; }
    IReadOnlyList<ApiInfo> Infos { get; }

    IApiResult AddError(ApiError error);

    IApiResult AddInfo(ApiInfo info);
}

public class ApiResult : IApiResult
{
    public bool Success => _errors.Count == 0;
    public int Status { get; private set; }
    public IReadOnlyList<ApiError> Errors => _errors.AsReadOnly();
    public IReadOnlyList<ApiInfo> Infos => _infos.AsReadOnly();

    protected readonly List<ApiError> _errors = [];
    protected readonly List<ApiInfo> _infos = [];

    public ApiResult(IResultBase result, HttpStatusCode statusCode)
    {
        Status = (int)statusCode;

        if (result.Errors is not null)
        {
            _errors.AddRange(result.Errors.ToApiError());
            // Ajusta o status para o erro mais grave encontrado
            Status = (_errors.Count > 0 ? _errors.MaxBy(e => e.Status)?.Status : Status) ?? Status;
        }
    }

    public ApiResult(HttpStatusCode statusCode, IEnumerable<ApiError> errors)
    {
        Status = (int)statusCode;

        if (errors is not null)
        {
            _errors.AddRange(errors);
            Status = (_errors.Count > 0 ? _errors.MaxBy(e => e.Status)?.Status : Status) ?? Status;
        }
    }

    public IApiResult SetStatus(int statusCode)
    {
        Status = statusCode;
        return this;
    }

    public IApiResult SetStatus(HttpStatusCode statusCode)
    {
        return SetStatus((int)statusCode);
    }

    public IApiResult AddError(ApiError error)
    {
        _errors.Add(error);
        return this;
    }

    public IApiResult AddInfo(ApiInfo info)
    {
        _infos.Add(info);
        return this;
    }

    public static ApiResult Ok()
    {
        return new(HttpStatusCode.OK, []);
    }

    // Método estático para facilitar a criação do genérico sem instanciar a classe
    public static ApiResult<TValue> Ok<TValue>(TValue value)
    {
        return new(value, HttpStatusCode.OK, []);
    }
}

public class ApiResult<TValue> : ApiResult
{
    // Esta é a propriedade que o ToActionResult procura via Reflexão
    public TValue? Data => _value;

    protected readonly TValue? _value;

    public ApiResult(IResultBase<TValue> result, HttpStatusCode statusCode)
        : base(result, statusCode)
    {
        if (result.IsSuccess)
        {
            _value = result.Value;
        }
    }

    public ApiResult(TValue value, HttpStatusCode statusCode, IEnumerable<ApiError> errors)
        : base(statusCode, errors)
    {
        _value = value;
    }

    // CORREÇÃO CS0693: Removido o <TValue> do nome do método, pois a classe já o define.
    public static ApiResult<TValue> Ok(TValue value)
    {
        return new ApiResult<TValue>(value, HttpStatusCode.OK, []);
    }
}