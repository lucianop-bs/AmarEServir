using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
        // Log detalhado para o desenvolvedor (no console ou arquivo)
        _logger.LogError(exception, "Erro Crítico Capturado: {Message}", exception.Message);

        const int statusCode = StatusCodes.Status500InternalServerError;

        // Retorno simplificado para o cliente/Swagger
        var response = new
        {
            status = statusCode,
            message = "Ocorreu um erro interno inesperado no servidor."
        };

        httpContext.Response.StatusCode = statusCode;

        // Envia o JSON para o cliente
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        // Retornar 'true' avisa ao .NET que a exceção foi tratada com sucesso
        return true;
    }
}