using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Errors;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Auth.API.Application.Common;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = failures
                .Select(f => new Error(
                    f.ErrorCode ?? "VALIDATION_ERROR",
                    f.ErrorMessage,
                    ErrorType.Validation))
                .ToList();

            if (typeof(TResponse) == typeof(Result))
            {
                return (Result.Fail(errors) as TResponse)!;
            }

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultValueType = typeof(TResponse).GetGenericArguments()[0];

                // ✅ SOLUÇÃO: Buscar especificamente métodos genéricos
                var failMethod = typeof(Result)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(m =>
                        m.Name == nameof(Result.Fail) &&
                        m.IsGenericMethodDefinition &&  // ← CRUCIAL: Só métodos genéricos
                        m.GetParameters().Length == 1 &&
                        m.GetParameters()[0].ParameterType == typeof(IEnumerable<IError>))
                    .FirstOrDefault();

                if (failMethod != null)
                {
                    var specificGenericFailMethod = failMethod.MakeGenericMethod(resultValueType);
                    var result = specificGenericFailMethod.Invoke(null, new object[] { errors });
                    return (result as TResponse)!;
                }

                throw new InvalidOperationException(
                    $"ValidationBehavior: Não foi possível criar Result<{resultValueType.Name}> com erros de validação.");
            }

            throw new InvalidOperationException(
                $"ValidationBehavior: Tipo '{typeof(TResponse).Name}' não suportado. Use Result ou Result<T>.");
        }

        return await next();
    }
}