using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Modulio.Application.Abstractions.Services;

namespace Modulio.Persistence.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ModulioDbContext>
    {
        public ModulioDbContext CreateDbContext(string[] args)
        {
            // Build configuration - look for appsettings in the API project
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            // If we're in the Persistence project, go up to find the API project
            if (basePath.EndsWith("Modulio.Persistence"))
            {
                basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Modulio.Api");
            }
            else if (!basePath.EndsWith("Modulio.Api"))
            {
                // Try to find the API project
                var apiPath = Path.Combine(basePath, "Modulio.Api");
                if (Directory.Exists(apiPath))
                {
                    basePath = apiPath;
                }
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ModulioDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string 'DefaultConnection' not found. Base path: {basePath}");
            }

            optionsBuilder.UseSqlServer(connectionString, b =>
            {
                b.MigrationsAssembly(typeof(ModulioDbContext).Assembly.FullName);
                b.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
            });

            // Create mock services for design time
            var mockCurrentUserService = new DesignTimeCurrentUserService();
            var mockDateTimeService = new DesignTimeDateTimeService();

            return new ModulioDbContext(optionsBuilder.Options, mockCurrentUserService, mockDateTimeService);
        }
    }

    // Mock services for design time
    internal class DesignTimeCurrentUserService : ICurrentUserService
    {
        public int? UserId => 0;
        public string? UserName => "system@modulio.com";
        public bool IsAuthenticated => false;
        public string? IpAddress => "0.0.0.0";
        public IEnumerable<string> Roles => Enumerable.Empty<string>();
        public bool IsInRole(string roleName) => false;
    }

    internal class DesignTimeDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
        public TimeOnly TimeOfDay => TimeOnly.FromDateTime(DateTime.Now);
    }
}