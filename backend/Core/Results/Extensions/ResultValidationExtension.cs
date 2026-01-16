using AmarEServir.Core.Results.Base;
using AmarEServir.Core.Results.Errors;

namespace AmarEServir.Core.Results.Extensions
{
    public static class ResultValidation
    {

        public static Result ValidateFailFast(params Func<Result>[] validators)
        {
            foreach (var validator in validators)
            {
                var result = validator();
                if (!result.IsSuccess)
                    return result;
            }

            return Result.Ok();
        }

        public static Result ValidateCollectErrors(params Func<Result>[] validators)
        {
            var errors = new List<IError>();

            foreach (var validator in validators)
            {
                var result = validator();
                if (!result.IsSuccess)
                    errors.AddRange(result.Errors);
            }

            return errors.Count == 0
                ? Result.Ok()
                : Result.Fail(errors);
        }
    }
}
