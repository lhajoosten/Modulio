using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Modulio.Api.Filters;
using Modulio.Application;
using Modulio.Infrastructure;
using Serilog;
using Serilog.Events;

namespace Modulio.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog early for bootstrap logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                Log.Information("Starting Modulio API");

                var builder = WebApplication.CreateBuilder(args);
                var services = builder.Services;
                var configuration = builder.Configuration;

                // Replace default logging with Serilog
                builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext());

                // Add services to the container.
                services.AddControllers(options =>
                {
                    options.Filters.Add<UnhandeledExceptionFilter>();   // Custom filter to handle unhandled exceptions globally
                });

                services.AddEndpointsApiExplorer();
                services.AddOpenApi();

                // Add HTTP Context Accessor for current user service
                services.AddHttpContextAccessor();

                // Register services to the DI container
                services.AddApiServices();                              // Custom API services
                services.AddInfrastructure(configuration);              // Custom Infrastructure services
                services.AddApplication();                              // Custom Application services

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.UseExceptionHandler("/error-local-development"); // Custom error handling endpoint for development
                }
                else
                {
                    app.UseExceptionHandler("/error");                  // Custom error handling endpoint
                    app.UseHsts();                                      // Use HSTS in production
                }

                // Add Serilog request logging
                app.UseSerilogRequestLogging(options =>
                {
                    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value!);
                        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.FirstOrDefault()!);
                        diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress?.ToString()!);

                        if (httpContext.User.Identity?.IsAuthenticated == true)
                        {
                            diagnosticContext.Set("UserId", httpContext.User.FindFirst("sub")?.Value! ?? httpContext.User.FindFirst("id")?.Value!);
                            diagnosticContext.Set("UserName", httpContext.User.Identity.Name!);
                        }
                    };

                    // Configure what gets logged
                    options.GetLevel = (httpContext, elapsed, ex) =>
                    {
                        if (ex != null)
                            return LogEventLevel.Error;

                        if (httpContext.Response.StatusCode > 499)
                            return LogEventLevel.Error;

                        if (httpContext.Response.StatusCode > 399)
                            return LogEventLevel.Warning;

                        if (elapsed > 5000) // Slow requests
                            return LogEventLevel.Warning;

                        return LogEventLevel.Information;
                    };
                });

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                // Health check endpoints
                app.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                app.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                app.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = _ => false // Always healthy for liveness
                });

                // Optional: Health checks UI (only in development)
                if (app.Environment.IsDevelopment())
                {
                    app.MapHealthChecksUI(options =>
                    {
                        options.UIPath = "/health-ui";
                        options.ApiPath = "/health-ui-api";
                    });
                }

                Log.Information("Modulio API started successfully on {Environment}", app.Environment.EnvironmentName);
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Modulio API terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}