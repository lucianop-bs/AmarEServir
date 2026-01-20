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
                    f.ErrorCode,
                    f.ErrorMessage,
                    ErrorType.Validation))
                .ToList();

            if (typeof(TResponse) == typeof(Result))
            {
                return (Result.Fail(errors) as TResponse)!;
            }

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultValueType = typeof(TResponse).GetGenericArguments()[0];

                var failMethod = typeof(Result)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m =>
                        m.Name == nameof(Result.Fail) &&
                        m.IsGenericMethod &&
                        m.GetParameters().Length == 1 &&

                        m.GetParameters()[0].ParameterType.IsAssignableFrom(typeof(List<Error>))
                    );

                if (failMethod != null)
                {
                    var specificGenericFailMethod = failMethod.MakeGenericMethod(resultValueType);
                    var result = specificGenericFailMethod.Invoke(null, new object[] { errors });
                    return (result as TResponse)!;
                }

                throw new InvalidOperationException($"O comando {typeof(TRequest).Name} precisa retornar Result ou Result<T> para usar a validação automática.");
            }

            throw new InvalidOperationException("Comando com erro de validação não retorna um tipo Result suportado.");
        }

        return await next();
    }
}