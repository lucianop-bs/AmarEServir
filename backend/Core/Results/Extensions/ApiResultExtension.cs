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
            var errors = apiResult.Errors;
            if (errors?.Count == 1)
            {
                return new ObjectResult(new
                {
                    code = apiResult.Status,

                    message = errors?.First().Detail
                })
                {
                    StatusCode = apiResult.Status
                };
            }
            else if (errors?.Count > 1)
            {
                return new ObjectResult(new
                {
                    errors = errors.Select(e => new
                    {
                        code = apiResult.Status,
                        message = e.Detail
                    }).ToArray()
                })
                {
                    StatusCode = apiResult.Status
                };
            }
            return new ObjectResult(new
            {
                code = apiResult.Status,
                message = "Ocorreu um erro inesperado."
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