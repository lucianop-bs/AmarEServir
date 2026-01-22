namespace AmarEServir.Core.Results.Errors
{
    public record Error(
        string Code,
        string Message,
        ErrorType Type
        ) : IError;
}