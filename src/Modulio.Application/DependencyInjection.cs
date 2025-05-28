using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Modulio.Application.Behaviors;
using System.Reflection;

namespace Modulio.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            // Register FluentValidation validators
            services.AddValidatorsFromAssembly(assembly);

            // Register MediatR behaviors in the correct order
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // Register AutoMapper
            services.AddAutoMapper(assembly);

            return services;
        }

        private static IServiceCollection AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            // Register FluentValidation validators
            var validatorTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Validator"))
                .ToList();

            foreach (var validatorType in validatorTypes)
            {
                var interfaceType = validatorType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                if (interfaceType != null)
                {
                    services.AddTransient(interfaceType, validatorType);
                }
            }

            return services;
        }
    }
}