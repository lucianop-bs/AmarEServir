using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.UpdateCell
{
    public class UpdateCellCommandValidator : AbstractValidator<UpdateCellCommand>
    {

        public UpdateCellCommandValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage(CellError.IdRequired.Message)
               .WithErrorCode(CellError.IdRequired.Code);

            RuleFor(x => x.Name)
            .NotEmpty().WithMessage(CellError.NameRequired.Message)
            .WithErrorCode(CellError.NameRequired.Code)
            .Length(3, 100).WithMessage(CellError.InvalidNameLength.Message)
            .WithErrorCode(CellError.InvalidNameLength.Code);

            RuleFor(x => x.LiderId)
                .NotEmpty().WithMessage(CellError.LeaderRoleRequired.Message)
                .WithErrorCode(CellError.LeaderRoleRequired.Code)
                .NotEqual(Guid.Empty).WithMessage(CellError.LeaderInvalid.Message)
                .WithErrorCode(CellError.LeaderInvalid.Code);
        }
    }
}
