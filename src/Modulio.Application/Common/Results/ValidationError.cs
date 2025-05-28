namespace Modulio.Application.Common.Results
{
    /// <summary>
    /// Represents a validation error
    /// </summary>
    public record ValidationError : Error
    {
        /// <summary>
        /// Creates a new validation error
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="message">The error message</param>
        /// <param name="propertyName">The name of the property that failed validation</param>
        public ValidationError(string code, string message, string propertyName)
            : base(code, message)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the name of the property that failed validation
        /// </summary>
        public string PropertyName { get; }
    }
}
