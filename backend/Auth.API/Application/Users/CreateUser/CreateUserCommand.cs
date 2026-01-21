using AmarEServir.Core.Results.Base;
using Auth.API.Application.Users.Dtos;
using MediatR;

namespace Auth.API.Application.Users.CreateUser
{
    public record class CreateUserCommand(CreateUserRequestDto User) : IRequest<Result<UserResponseDto>>;
}
