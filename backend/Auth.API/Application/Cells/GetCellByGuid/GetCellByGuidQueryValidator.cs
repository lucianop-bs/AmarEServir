using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Cells.GetCellByGuid
{
    public class GetCellByGuidQueryValidator : AbstractValidator<GetCellByGuidQuery>
    {
        public GetCellByGuidQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(CellError.IdRequired.Message)
                .WithErrorCode(CellError.IdRequired.Code);
        }
    }
}