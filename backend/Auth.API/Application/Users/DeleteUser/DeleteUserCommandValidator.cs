using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {

        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
<<<<<<< HEAD
                .NotEmpty().WithMessage(UserErrors.Account.IdRequired.Message)
                .WithErrorCode(UserErrors.Account.IdRequired.Code);

=======
                .NotEmpty().WithMessage(CellError.IdRequired.Message)
                .WithErrorCode(CellError.IdRequired.Code);
          
>>>>>>> a208f52ad21eb976f79e0dff1d708f3347d92cd9
        }
    }
}
