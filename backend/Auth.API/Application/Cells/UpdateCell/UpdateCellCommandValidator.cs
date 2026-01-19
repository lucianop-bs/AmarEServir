using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.UpdateCell
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateCellCommand>
    {

        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage(CellError.IdRequired.Message)
               .WithErrorCode(CellError.IdRequired.Code)
               .NotNull().WithMessage(CellError.IdRequired.Message)
               .WithErrorCode(CellError.IdRequired.Code);

            RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CellError.InvalidName.Message)
            .WithErrorCode(CellError.InvalidName.Code)
            .Length(3, 100).WithMessage(CellError.InvalidNameLength.Message)
            .WithErrorCode(CellError.InvalidNameLength.Code);

            RuleFor(x => x.LiderId)
                .NotEmpty().WithMessage(CellError.LeaderRequired.Message)
                .WithErrorCode(CellError.LeaderRequired.Code)
                .NotEqual(Guid.Empty).WithMessage(CellError.LeaderInvalid.Message)
                .WithErrorCode(CellError.LeaderInvalid.Code);
        }
    }
}
