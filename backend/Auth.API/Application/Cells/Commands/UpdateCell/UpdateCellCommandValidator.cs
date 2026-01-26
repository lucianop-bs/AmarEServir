using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.Commands.UpdateCell
{
    public class UpdateCellCommandValidator : AbstractValidator<UpdateCellCommand>
    {
        public UpdateCellCommandValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty()
               .WithMessage(CellError.IdRequired.Message)
               .WithErrorCode(CellError.IdRequired.Code);

            RuleFor(x => x.Cell.Name)
                .Length(3, 100)
                .WithMessage(CellError.InvalidNameLength.Message)
                .WithErrorCode(CellError.InvalidNameLength.Code)
                .When(x => !string.IsNullOrWhiteSpace(x.Cell.Name));

            RuleFor(x => x.Cell.LeaderId)
                .NotEqual(Guid.Empty)
                .WithMessage(CellError.LeaderInvalid.Message)
                .WithErrorCode(CellError.LeaderInvalid.Code)
                .When(x => x.Cell.LeaderId.HasValue);
        }
    }
}