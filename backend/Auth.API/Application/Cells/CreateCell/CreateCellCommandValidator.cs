using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.CreateCell
{
    public class CreateCellCommandValidator : AbstractValidator<CreateCellCommand>
    {

        public CreateCellCommandValidator()
        {

            RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CellError.InvalidName.Message)
            .WithErrorCode(CellError.InvalidName.Code)
            .Length(3, 100).WithMessage(CellError.InvalidNameLength.Message)
            .WithErrorCode(CellError.InvalidNameLength.Code);

            RuleFor(x => x.LeaderId)
                .NotEmpty().WithMessage(CellError.LeaderRequired.Message)
                .WithErrorCode(CellError.LeaderRequired.Code)
                .NotEqual(Guid.Empty).WithMessage(CellError.LeaderInvalid.Message)
                .WithErrorCode(CellError.LeaderInvalid.Code);
        }
    }
}
