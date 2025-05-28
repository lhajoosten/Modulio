using MediatR;
using Microsoft.Extensions.Logging;
using Modulio.Application.Abstractions.CQRS;
using Modulio.Application.Abstractions.Services;
using Newtonsoft.Json;

namespace Modulio.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for auditing
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<AuditBehavior<TRequest, TResponse>> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;

        // Commands to be audited
        private static readonly HashSet<Type> _commandTypes = new HashSet<Type>
        {
            typeof(ICommand),
        };

        public AuditBehavior(
            ILogger<AuditBehavior<TRequest, TResponse>> logger,
            ICurrentUserService currentUserService,
            IAuditService auditService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _auditService = auditService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Only audit commands, not queries
            if (!ShouldAudit(request))
            {
                return await next();
            }

            string requestName = typeof(TRequest).Name;

            try
            {
                // Execute the command
                var response = await next();

                // Create audit record after successful execution
                var auditRecord = AuditRecord.Create(
                    action: requestName,
                    entityType: typeof(TRequest).Name,
                    userName: _currentUserService.UserName,
                    ipAddress: _currentUserService.IpAddress,
                    details: null,
                    userId: _currentUserService.UserId?.ToString(),
                    data: SanitizeAndSerialize(request),
                    status: AuditStatus.Success
                );

                // Record the audit asynchronously (don't wait for it)
                _ = _auditService.RecordAuditAsync(auditRecord, cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                // Record the exception in the audit log
                var auditRecord = AuditRecord.Create(
                    action: requestName,
                    entityType: typeof(TRequest).Name,
                    userName: _currentUserService.UserName,
                    ipAddress: _currentUserService.IpAddress,
                    details: null,
                    userId: _currentUserService.UserId?.ToString(),
                    data: SanitizeAndSerialize(request),
                    status: AuditStatus.Failure
                );
                auditRecord.ErrorMessage = ex.Message;

                // Record the audit asynchronously (don't wait for it)
                _ = _auditService.RecordAuditAsync(auditRecord, cancellationToken);

                // Re-throw the exception
                throw;
            }
        }

        private static bool ShouldAudit(TRequest request)
        {
            // Check if it's an ICommand or ICommand<T>
            var requestType = request.GetType();

            foreach (var type in _commandTypes)
            {
                if (type.IsAssignableFrom(requestType))
                    return true;

                if (type.IsGenericType && requestType.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type))
                    return true;
            }

            return false;
        }

        private string SanitizeAndSerialize(TRequest request)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    MaxDepth = 5 // Limit the depth to prevent large objects
                };

                return JsonConvert.SerializeObject(request, settings);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to serialize request for audit log");
                return $"{{\"error\": \"Failed to serialize request: {ex.Message}\"}}";
            }
        }
    }
}
