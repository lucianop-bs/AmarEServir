using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using Auth.API.Domain;
using Auth.API.Domain.Enums;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{
    public record class CreateUserCommand(CreateUserRequest User) : IRequest<Result<UserModelView>>;

    public record class CreateUserRequest(string Name, string Email, string Phone, string Password, UserRole Role, Address Address);
}
