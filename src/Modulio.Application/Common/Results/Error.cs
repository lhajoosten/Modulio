namespace Modulio.Application.Common.Results
{
    /// <summary>
    /// Represents an error that occurred during an operation
    /// </summary>
    public record Error
    {
        /// <summary>
        /// Gets a special error that represents no error
        /// </summary>
        public static readonly Error None = new Error(string.Empty, string.Empty);

        /// <summary>
        /// Gets a standard error for when an entity was not found
        /// </summary>
        public static readonly Error NotFound = new Error("General.NotFound", "The specified resource was not found.");

        /// <summary>
        /// Gets a standard error for when validation fails
        /// </summary>
        public static readonly Error ValidationFailed = new Error("General.ValidationFailed", "One or more validation errors occurred.");

        /// <summary>
        /// Gets a standard error for when an unauthorized operation is attempted
        /// </summary>
        public static readonly Error Unauthorized = new Error("General.Unauthorized", "The current user is not authorized to perform this operation.");

        /// <summary>
        /// Gets a standard error for when a conflict occurs
        /// </summary>
        public static readonly Error Conflict = new Error("General.Conflict", "A conflict occurred while processing the request.");

        /// <summary>
        /// Gets a standard error for when an operation fails
        /// </summary>
        public static readonly Error OperationFailed = new Error("General.OperationFailed", "The operation failed to complete.");

        /// <summary>
        /// Creates a new error
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="message">The error message</param>
        /// <param name="details">Optional details about the error</param>
        public Error(string code, string message, string details = "")
        {
            Code = code;
            Message = message;
            Details = details;
        }

        /// <summary>
        /// Gets the error code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets additional details about the error
        /// </summary>
        public string Details { get; }

        /// <summary>
        /// Creates a detailed error based on this error
        /// </summary>
        /// <param name="details">The details to add to the error</param>
        /// <returns>A new error with the same code and message, but with the specified details</returns>
        public Error WithDetails(string details)
        {
            return new Error(Code, Message, details);
        }
    }
}
