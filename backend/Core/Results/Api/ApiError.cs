namespace AmarEServir.Core.Results.Api
{
    public sealed record ApiError(string Title, string Detail, int Status, string Type)
    { }
}