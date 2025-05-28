namespace Modulio.Application.Common.Results
{
    /// <summary>
    /// Represents the result of an operation, which can be a success or a failure
    /// </summary> 
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("A successful result cannot have an error.");

            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("A failure result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Gets a value indicating whether the result is a success
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the result is a failure
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the error associated with the result, if any
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Creates a successful result
        /// </summary>
        /// <returns>A successful result</returns>
        public static Result Success() => new Result(true, Error.None);

        /// <summary>
        /// Creates a failure result with the specified error
        /// </summary>
        /// <param name="error">The error that caused the failure</param>
        /// <returns>A failure result with the specified error</returns>
        public static Result Failure(Error error) => new Result(false, error);

        /// <summary>
        /// Creates a successful result with the specified value
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="value">The value</param>
        /// <returns>A successful result with the specified value</returns>
        public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);

        /// <summary>
        /// Creates a failure result with the specified error
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="error">The error that caused the failure</param>
        /// <returns>A failure result with the specified error</returns>
        public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default!, false, error);
    }

    /// <summary>
    /// Represents the result of an operation that returns a value, which can be a success or a failure
    /// </summary>
    /// <typeparam name="TValue">The type of the value</typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        protected internal Result(TValue value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value of the result, if it is a success
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when trying to get the value of a failure result</exception>
        public TValue Value
        {
            get
            {
                if (IsFailure)
                    throw new InvalidOperationException("Cannot access the value of a failure result.");

                return _value;
            }
        }

        public static implicit operator Result<TValue>(TValue value) => Success(value);
    }
}
