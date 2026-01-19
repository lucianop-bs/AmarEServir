using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {

        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(CellError.IdRequired.Message)
                .WithErrorCode(CellError.IdRequired.Code);
          
        }
    }
}
