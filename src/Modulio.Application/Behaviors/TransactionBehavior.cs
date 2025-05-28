using MediatR;
using Microsoft.Extensions.Logging;
using Modulio.Application.Abstractions.CQRS;
using Modulio.Application.Abstractions.Persistence;
using Modulio.Application.Common.Results;

namespace Modulio.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for transaction management
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(
            IUnitOfWork unitOfWork,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Only apply transactions to commands (not queries)
            if (request is not ICommand)
            {
                return await next();
            }

            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Beginning transaction for {RequestName}", requestName);

            try
            {
                // Execute the request handler
                var response = await next();

                // Save changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Committed transaction for {RequestName}", requestName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during transaction for {RequestName}", requestName);

                // If the response is a Result type, return a failure
                if (typeof(TResponse) == typeof(Result) ||
                    (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>)))
                {
                    if (typeof(TResponse) == typeof(Result))
                    {
                        return (TResponse)(object)Result.Failure(Error.OperationFailed.WithDetails(ex.Message));
                    }
                    else
                    {
                        var resultType = typeof(TResponse).GetGenericArguments()[0];
                        var failureMethod = typeof(Result)
                            .GetMethod(nameof(Result.Failure))!
                            .MakeGenericMethod(resultType);

                        return (TResponse)failureMethod.Invoke(null, [Error.OperationFailed.WithDetails(ex.Message)])!;
                    }
                }

                throw; // Re-throw if not a Result
            }
        }
    }
}
