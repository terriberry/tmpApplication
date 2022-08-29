using System.Reflection;
using CoreApplicationFilterVal_10.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Application.DI.ApplicationDependencyInjection", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<IApplicationService>()
                .AddClasses(classes => classes.AssignableTo<IApplicationService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );


        AddCustomApplication(services, configuration);

        return services;
    }

    [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
    private static void AddCustomApplication(IServiceCollection services, IConfiguration configuration)
    {
        // Configure your own custom dependency injections here...
    }
}
