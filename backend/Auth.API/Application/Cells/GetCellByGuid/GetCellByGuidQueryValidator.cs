using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.GetCellByGuid
{
    public class DeleteCellCommandValidator : AbstractValidator<GetCellByGuidQuery>
    {

        public DeleteCellCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(CellError.IdRequired.Message)
                .WithErrorCode(CellError.IdRequired.Code)
                .NotNull().WithMessage(CellError.IdRequired.Message)
                .WithErrorCode(CellError.IdRequired.Code);
        }
    }
}
