using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.DTOs.AutoMapper.MappingProfile", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(typeof(MappingProfile).Assembly);
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var typesFrom = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        var typesTo = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>)
                            && !t.IsAbstract
                            && !t.IsInterface)).ToList();

        foreach (var type in typesTo)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapTo`1").GetMethod("MappingTo");

            methodInfo?.Invoke(instance, new object[] { this });
        }

        foreach (var type in typesFrom)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapFrom`1").GetMethod("MappingFrom");

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
