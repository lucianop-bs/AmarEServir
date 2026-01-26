using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)

                .NotEmpty()
                .WithMessage(UserErrors.Account.IdRequired.Message)
                .WithErrorCode(UserErrors.Account.IdRequired.Code);
        }
    }
}