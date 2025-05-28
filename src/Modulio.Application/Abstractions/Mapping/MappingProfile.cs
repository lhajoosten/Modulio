using AutoMapper;
using System.Reflection;

namespace Modulio.Application.Abstractions.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)));

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var method = type.GetMethod("Mapping");
                method?.Invoke(instance, new object[] { this });
            }
        }
    }
}
