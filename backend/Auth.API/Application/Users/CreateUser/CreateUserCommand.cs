using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using Auth.API.Domain.Enums;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{
    public record class CreateUserCommand(CreateUserRequest User) : IRequest<Result<UserResponse>>;

    public record class CreateUserRequest(
        string Name,
        string Email,
        string Phone,
        string Password,
        UserRole Role,
        AddressRequest Address);

    public record class AddressRequest(
        string Rua,
        string Quadra,
        string Numero,
        string Bairro,
        string Cidade,
        string Estado,
        string Complemento,
        string Cep
    );
}
