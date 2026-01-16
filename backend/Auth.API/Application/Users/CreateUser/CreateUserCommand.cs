using AmarEServir.Core.Results.Base;
using Auth.API.Domain;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{
    public record class CreateUserCommand(string Name, string Email, string Phone, string Password, Guid? CelulaId, string? CellName, UserRole Role, AddressCommand Address) : IRequest<Result<CreatedUserResponse>>;
    public record class AddressCommand(string Rua, string Quadra, string Numero, string Bairro, string Cidade, string Estado, string Complemento, string Cep);
}
