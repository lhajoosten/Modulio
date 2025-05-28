using Microsoft.Extensions.Logging;

namespace Modulio.Application.Extensions
{
    public static class LoggingExtensions
    {
        public static IDisposable? BeginPropertyScope(this ILogger logger, string name, object? value)
        {
            return logger.BeginScope(new Dictionary<string, object?> { [name] = value });
        }

        public static IDisposable? BeginPropertyScope(this ILogger logger, params (string Name, object? Value)[] properties)
        {
            return logger.BeginScope(properties.ToDictionary(p => p.Name, p => p.Value));
        }

        public static void LogCommandExecution<T>(this ILogger<T> logger, string commandName, TimeSpan duration, bool success)
        {
            using (logger.BeginPropertyScope("CommandName", commandName))
            using (logger.BeginPropertyScope("Duration", duration.TotalMilliseconds))
            using (logger.BeginPropertyScope("Success", success))
            {
                if (success)
                {
                    logger.LogInformation("Command {CommandName} executed successfully in {Duration:0.00}ms", commandName, duration.TotalMilliseconds);
                }
                else
                {
                    logger.LogWarning("Command {CommandName} failed after {Duration:0.00}ms", commandName, duration.TotalMilliseconds);
                }
            }
        }

        public static void LogQueryExecution<T>(this ILogger<T> logger, string queryName, TimeSpan duration, int resultCount)
        {
            using (logger.BeginPropertyScope("QueryName", queryName))
            using (logger.BeginPropertyScope("Duration", duration.TotalMilliseconds))
            using (logger.BeginPropertyScope("ResultCount", resultCount))
            {
                logger.LogInformation("Query {QueryName} executed in {Duration:0.00}ms returning {ResultCount} results",
                    queryName, duration.TotalMilliseconds, resultCount);
            }
        }
    }
}