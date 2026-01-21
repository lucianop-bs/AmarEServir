using AmarEServir.Core.Results.Base;
using Auth.API.Domain;
using Auth.API.Domain.Enums;
using MediatR;

namespace Auth.API.Application.Users.UpdateUser
{
    public record class UpdateUserCommand(Guid Id, UpdateUserRequest User) : IRequest<Result>;

    public record class UpdateUserRequest(string? Name, string? Email, string? Phone, AddressRequest? Address, UserRole? Role);
    public record class AddressRequest(
        string? Rua,
        string? Quadra,
        string? Numero,
        string? Bairro,
        string? Cidade,
        string? Estado,
        string? Complemento,
        string? Cep
    );
}

namespace Auth.API.Application.Users.UpdateUser.Extensions
{
    public static class AddressRequestExtensions
    {
        public static Address ToAddress(this AddressRequest request)
        {
            return new Address(
                request.Rua ?? string.Empty,
                request.Quadra ?? string.Empty,
                request.Numero ?? string.Empty,
                request.Bairro ?? string.Empty,
                request.Cidade ?? string.Empty,
                request.Estado ?? string.Empty,
                request.Complemento ?? string.Empty,
                request.Cep ?? string.Empty
            );
        }
    }
}
