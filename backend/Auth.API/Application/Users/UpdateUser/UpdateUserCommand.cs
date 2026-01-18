using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Models;
using Auth.API.Domain;
using MediatR;

namespace Auth.API.Application.Users.UpdateUser
{
    public record class UpdateUserCommand(Guid Id, UpdateUserRequest User) : IRequest<Result>;

    public record class UpdateUserRequest(string Name, string Email, string Phone, AddressModelView Address, UserRole Role);
}