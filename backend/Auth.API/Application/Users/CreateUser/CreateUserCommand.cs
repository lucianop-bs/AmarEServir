using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using Auth.API.Domain;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{
    public record class CreateUserCommand(string Name, string Email, string Phone, string Password, UserRole Role, AddressCommand Address) : IRequest<Result<UserModelView>>;
    public record class AddressCommand(string Rua, string Quadra, string Numero, string Bairro, string Cidade, string Estado, string Complemento, string Cep);
}
