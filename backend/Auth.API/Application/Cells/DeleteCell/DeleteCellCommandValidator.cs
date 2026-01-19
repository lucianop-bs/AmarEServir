using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.DeleteCell
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteCellCommand>
    {

        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(CellError.IdRequired.Message)
                .WithErrorCode(CellError.IdRequired.Code);
        }
    }
}
