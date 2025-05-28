using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Modulio.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for logging
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var requestId = Guid.NewGuid().ToString();

            _logger.LogInformation("[{RequestId}] Handling {RequestName}", requestId, requestName);

            TResponse response;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                response = await next();
                stopwatch.Stop();

                _logger.LogInformation("[{RequestId}] Completed {RequestName} in {ElapsedMilliseconds}ms",
                    requestId, requestName, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex, "[{RequestId}] Error handling {RequestName} after {ElapsedMilliseconds}ms: {ErrorMessage}",
                    requestId, requestName, stopwatch.ElapsedMilliseconds, ex.Message);

                throw;
            }
        }
    }
}
