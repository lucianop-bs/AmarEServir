using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.CreateCell
{
    public class CreateCellCommandValidator : AbstractValidator<CreateCellCommand>
    {
        public CreateCellCommandValidator()
        {
            RuleFor(x => x.Cell.Name)
                .NotNull()
                .WithMessage(CellError.NameRequired.Message)
                .NotEmpty()
                .WithMessage(CellError.NameRequired.Message)
                .WithErrorCode(CellError.NameRequired.Code)
                .Length(3, 100)
                .WithMessage(CellError.InvalidNameLength.Message)
                .WithErrorCode(CellError.InvalidNameLength.Code);

            RuleFor(x => x.Cell.LeaderId)
                .NotNull()
                 .WithMessage(CellError.LeaderInvalid.Message)
                 .NotEmpty()
                 .WithMessage(CellError.LeaderRoleRequired.Message)
                 .WithErrorCode(CellError.LeaderRoleRequired.Code)
                 .NotEqual(Guid.Empty)
                 .WithMessage(CellError.LeaderInvalid.Message)
                 .WithErrorCode(CellError.LeaderInvalid.Code);
        }
    }
}