using AmarEServir.Core.Results.Errors;

namespace AmarEServir.Core.Results.Base
{
    public class Result : ResultBase
    {
        protected Result()
        { }

        protected Result(IError error)
        {
            AddError(error);
        }

        protected Result(IEnumerable<IError> errors)
        {
            AddErrors(errors);
        }

        public static Result Ok()
        {
            return new();
        }

        public static Result<TValue> Ok<TValue>(TValue value)
        {
            return Result<TValue>.Ok(value);
        }

        public static Result Fail(IError error)
        {
            return new(error);
        }

        public static Result Fail(IEnumerable<IError> errors)
        {
            return new(errors);
        }

        public static Result<TValue> Fail<TValue>(IError error)
        {
            return Result<TValue>.Fail(error);
        }

        public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors)
        {
            return Result<TValue>.Fail(errors);
        }
    }

    public class Result<TValue> : ResultBase, IResultBase<TValue>
    {
        private readonly TValue? _value;

        protected Result(TValue value) : base()
        {
            _value = value;
        }

        protected Result(IError error) : base()
        {
            AddError(error);
        }

        protected Result(IEnumerable<IError> errors) : base()
        {
            AddErrors(errors);
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Não se acessa o valor de uma falha.");

        public static Result<TValue> Ok(TValue value)
        {
            return new(value);
        }

        public static Result<TValue> Fail(IError error)
        {
            return new(error);
        }

        public static Result<TValue> Fail(IEnumerable<IError> errors)
        {
            return new(errors);
        }

        public static implicit operator Result<TValue>(TValue value)
        {
            return Ok(value);
        }
    }
}