using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Modulio.Application.Common.Results;

namespace Modulio.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for validation
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            _logger.LogDebug("Validating {RequestType}", typeof(TRequest).Name);

            // Create a validation context with the request
            var context = new ValidationContext<TRequest>(request);

            // Validate the request using all validators
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Combine all validation failures
            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            if (failures.Count != 0)
            {
                _logger.LogWarning("Validation failed for {RequestType} with {ErrorCount} errors",
                    typeof(TRequest).Name, failures.Count);

                // Convert validation failures to ValidationErrors
                var errors = failures
                    .Select(failure => new ValidationError(
                        failure.ErrorCode,
                        failure.ErrorMessage,
                        failure.PropertyName))
                    .ToList();

                // If response is a Result type, return validation error result
                if (typeof(TResponse).IsGenericType &&
                    (typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>) ||
                     typeof(TResponse) == typeof(Result)))
                {
                    // Create validation result with errors
                    var validationResult = ValidationResult.WithErrors(errors.ToArray());

                    // Create Result<T> or Result
                    var resultType = typeof(TResponse);
                    if (resultType == typeof(Result))
                    {
                        // Return a failed Result
                        return Result.Failure(Error.ValidationFailed) as TResponse;
                    }
                    else
                    {
                        // Get the generic type parameter of Result<T>
                        var resultValueType = resultType.GetGenericArguments()[0];

                        // Use reflection to create a typed Result<T>.Failure
                        var failureMethod = typeof(Result)
                            .GetMethod(nameof(Result.Failure))!
                            .MakeGenericMethod(resultValueType);

                        return failureMethod.Invoke(null, [Error.ValidationFailed]) as TResponse;
                    }
                }

                // If the type is not a Result, throw an exception
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
