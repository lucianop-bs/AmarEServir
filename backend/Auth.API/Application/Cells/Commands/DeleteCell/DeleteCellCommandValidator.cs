using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.Commands.DeleteCell
{
    public class DeleteCellCommandValidator : AbstractValidator<DeleteCellCommand>
    {
        public DeleteCellCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(CellError.IdRequired.Message)
                .WithErrorCode(CellError.IdRequired.Code);
        }
    }
}