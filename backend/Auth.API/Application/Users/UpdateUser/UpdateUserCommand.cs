using AmarEServir.Core.Results.Base;
using Auth.API.Domain;
using Auth.API.Domain.Enums;
using MediatR;

namespace Auth.API.Application.Users.UpdateUser
{
    public record class UpdateUserCommand(Guid Id, UpdateUserRequest User) : IRequest<Result>;

    public record class UpdateUserRequest(string Name, string Email, string Phone, string Password, Address Address, UserRole Role);
}