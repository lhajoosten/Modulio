using Modulio.Application.Abstractions.Services;
using Serilog.Core;
using Serilog.Events;

namespace Modulio.Api.Logging
{
    public class UserEnricher : ILogEventEnricher
    {
        private readonly ICurrentUserService _currentUserService;

        public UserEnricher(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_currentUserService.IsAuthenticated)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserId", _currentUserService.UserId));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserName", _currentUserService.UserName));
            }
        }
    }
}