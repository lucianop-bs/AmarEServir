namespace AmarEServir.Core.Results.Errors
{
    public enum ErrorType
    {
        Validation = 400,
        Unauthorized = 401,
        NotFound = 404,
        Conflict = 409,
        Internal = 500,
        Failure = 400
    }
}
