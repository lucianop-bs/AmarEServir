using Auth.API.Domain.Errors;
using FluentValidation;

namespace Auth.API.Application.Users.GetUserByGuid
{

    public class GetUserByGuidQueryValidator : AbstractValidator<GetUserByGuidQuery>
    {

        public GetUserByGuidQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(UserErrors.Account.IdRequired.Message)
                .WithErrorCode(UserErrors.Account.IdRequired.Code);

        }
    }
}
