using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Modulio.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for performance monitoring
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        private readonly Stopwatch _timer;

        // Threshold in milliseconds for warning about long-running requests
        private const int LongRunningThreshold = 500;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > LongRunningThreshold)
            {
                // Log a warning for long-running requests
                var requestName = typeof(TRequest).Name;
                var requestType = typeof(TRequest).ToString();

                _logger.LogWarning("Long running request: {RequestName} ({ElapsedMilliseconds} ms) {@Request}",
                    requestName, elapsedMilliseconds, request);
            }

            return response;
        }
    }
}
