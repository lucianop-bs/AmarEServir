

using AmarEServir.Core.Results.Api;
using AmarEServir.Core.Results.Errors;
using AmarEServir.Core.Results.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AmarEServir.Core.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }


    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        // Definimos o status como 500 (Erro Interno)
        int statusCode = StatusCodes.Status500InternalServerError;

        // Criamos o objeto de resposta exatamente no padrão que você pediu
        var response = new
        {
            status = statusCode,
            // Dica: Em produção, evite mostrar a exception.Message real para o usuário por segurança
            message = "Ocorreu um erro interno inesperado no servidor."
        };

        httpContext.Response.StatusCode = statusCode;

        // Escrevemos o JSON diretamente na resposta
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true; // Indica que a exceção foi tratada
    }
}



