using FluentValidation.Results;
using Modulio.Application.Common.Results;

namespace Modulio.Application.Extensions
{
    /// <summary>
    /// Extension methods for the Result class
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Converts a value to a successful result
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="value">The value to wrap in a result</param>
        /// <returns>A successful result containing the value</returns>
        public static Result<TValue> ToSuccess<TValue>(this TValue value) =>
            Result.Success(value);

        /// <summary>
        /// Creates a not found result
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="message">Optional custom message for the not found error</param>
        /// <returns>A failure result with a not found error</returns>
        public static Result<TValue> ToNotFound<TValue>(string? message = null) =>
            Result.Failure<TValue>(message == null
                ? Error.NotFound
                : new Error(Error.NotFound.Code, message));

        /// <summary>
        /// Creates an error result with a generic error message
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="message">The error message</param>
        /// <returns>A failure result with the specified error</returns>
        public static Result<TValue> ToError<TValue>(string message) =>
            Result.Failure<TValue>(new Error("General.Error", message));

        /// <summary>
        /// Converts FluentValidation validation failures to a validation error result
        /// </summary>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="failures">The validation failures</param>
        /// <returns>A failure result with validation errors</returns>
        public static Result<TValue> ToInvalid<TValue>(this IEnumerable<ValidationFailure> failures)
        {
            var validationErrors = failures
                .Select(f => new ValidationError("Validation.Error", f.ErrorMessage, f.PropertyName))
                .ToList();

            // Use a custom error that combines all validation errors
            var errorMessage = "One or more validation errors occurred.";
            var details = string.Join(Environment.NewLine, validationErrors.Select(e => $"{e.PropertyName}: {e.Message}"));
            var error = new Error(Error.ValidationFailed.Code, errorMessage, details);

            return Result.Failure<TValue>(error);
        }
    }
}
