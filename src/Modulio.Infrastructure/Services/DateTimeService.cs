using Modulio.Application.Abstractions.Services;

namespace Modulio.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly TimeOfDay => TimeOnly.FromDateTime(DateTime.Now);
    }
}
