using AmarEServir.Core.Results.Api;
using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmarEServir.Core.Results.Extensions;

public static class ApiResultExtensions
{
    public static ApiResult ToApiResult(this IResultBase result, HttpStatusCode statusCode = HttpStatusCode.NoContent)
    {
        return new ApiResult(result, statusCode);
    }

    public static ApiResult ToApiResult<TValue>(this IResultBase<TValue> result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (!result.IsSuccess)
        {
            var firstError = result.Errors?.FirstOrDefault();
            var errorStatus = (HttpStatusCode)GetStatusCode(firstError!);
            return new ApiResult<TValue>(result, errorStatus);
        }

        statusCode = result.Value is null ? HttpStatusCode.NoContent : statusCode;
        return new ApiResult<TValue>(result, statusCode);
    }

    public static ObjectResult ToActionResult(this IApiResult apiResult)
    {
        if (apiResult.Status >= 400)
        {

            var firstError = apiResult.Errors?.FirstOrDefault();

            return new ObjectResult(new
            {
                status = apiResult.Status,

                message = firstError?.Detail ?? "Ocorreu um erro inesperado."
            })
            {
                StatusCode = apiResult.Status
            };
        }

        if (apiResult.Status == StatusCodes.Status204NoContent)
        {
            return new ObjectResult(null) { StatusCode = StatusCodes.Status204NoContent };
        }

        var data = apiResult.GetType().GetProperty("Data")?.GetValue(apiResult);

        return new ObjectResult(data) { StatusCode = apiResult.Status };
    }
    public static ApiResult ToCreatedResult<TValue>(this IResultBase<TValue> result)
    {

        return result.ToApiResult(HttpStatusCode.Created);
    }

    public static IResult ToResult(this IApiResult apiResult)
    {
        if (apiResult.Status >= 400)
        {
            var firstError = apiResult.Errors?.FirstOrDefault();
            return Microsoft.AspNetCore.Http.Results.Json(new
            {
                status = apiResult.Status,
                message = firstError?.Detail ?? "Erro inesperado."
            }, statusCode: apiResult.Status);
        }

        if (apiResult.Status == StatusCodes.Status204NoContent)
            return Microsoft.AspNetCore.Http.Results.NoContent();

        var data = apiResult.GetType().GetProperty("Data")?.GetValue(apiResult);
        return Microsoft.AspNetCore.Http.Results.Json(data, statusCode: apiResult.Status);
    }

    private static int GetStatusCode(IError error)
    {
        if (error == null) return StatusCodes.Status500InternalServerError;

        return error.Type switch
        {
            ErrorType.Validation or ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    public static ApiError ToApiError(this IError error)
    {
        return new(GetTitle(error), error.Message, GetStatusCode(error), GetType(error));
    }

    public static IEnumerable<ApiError> ToApiError(this IEnumerable<IError> errors)
    {
        return errors?.Select(e => e.ToApiError()) ?? Enumerable.Empty<ApiError>();
    }

    private static string GetTitle(IError error)
    {
        return error.Type switch
        {
            ErrorType.Validation => "Bad Request",
            ErrorType.NotFound => "Not Found",
            _ => "Internal Server Error"
        };
    }

    private static string GetType(IError error)
    {
        return "https://tools.ietf.org/html/rfc7231#section-6";
    }
}