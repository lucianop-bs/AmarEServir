using AmarEServir.Core.Results.Errors;

namespace AmarEServir.Core.Results.Base
{
    public interface IResultBase
    {
        IReadOnlyCollection<IError> Errors { get; }
        bool IsSuccess { get; }
        bool IsFailure { get; }
    }

    public interface IResultBase<out TValue> : IResultBase
    {
        TValue Value { get; }
    }
    public abstract class ResultBase : IResultBase
    {
        private readonly List<IError> _errors = new();

        public bool IsSuccess => !_errors.Any();
        public bool IsFailure => _errors.Any();
        public IReadOnlyCollection<IError> Errors => _errors.AsReadOnly();

        protected void AddError(IError error)
        {
            if (error != null) _errors.Add(error);
        }

        protected void AddErrors(IEnumerable<IError> errors)
        {
            if (errors != null) _errors.AddRange(errors);
        }
    }
}
