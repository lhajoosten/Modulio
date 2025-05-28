namespace Modulio.Application.Common.Results
{
    /// <summary>
    /// Represents the result of a validation operation, which can contain multiple validation errors
    /// </summary>
    public sealed class ValidationResult : Result
    {
        private ValidationResult(List<ValidationError> errors)
            : base(false, Error.ValidationFailed)
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets the validation errors
        /// </summary>
        public IReadOnlyCollection<ValidationError> Errors { get; }

        /// <summary>
        /// Creates a successful validation result
        /// </summary>
        /// <returns>A successful validation result</returns>
        public static new ValidationResult Success() => new([]);

        /// <summary>
        /// Creates a failure validation result with the specified errors
        /// </summary>
        /// <param name="errors">The validation errors</param>
        /// <returns>A failure validation result with the specified errors</returns>
        public static ValidationResult WithErrors(params ValidationError[] errors) => new([.. errors]);
    }
}
