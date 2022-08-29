using System.Reflection;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.DI.DomainDependencyInjection", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.Scan(scan =>
            scan.FromAssemblyOf<IDomainService>()
                .AddClasses(classes => classes.AssignableTo<IDomainService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

        AddCustomDomain(services, configuration);

        return services;
    }

    [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
    private static void AddCustomDomain(IServiceCollection services, IConfiguration configuration)
    {
        // Configure your own custom dependency injections here...
    }
}
