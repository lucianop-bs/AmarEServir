namespace AmarEServir.Core.Results.Errors
{
    public interface IError
    {
        string Code { get; }
        string Message { get; }
        ErrorType Type { get; }
    }
}