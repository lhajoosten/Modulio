namespace Modulio.Application.Abstractions.Services
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
        DateTime Now { get; }
        DateOnly Today { get; }
        TimeOnly TimeOfDay { get; }
    }
}
