using System.Reflection;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using CoreApplicationFilterVal_10.Persistence.Persistence;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Persistence.DI.PersistenceDependencyInjection", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<ApplicationDbContext>()
                .AddClasses(classes => classes.AssignableTo<IRepository>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

        services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("CoreApplicationFilterVal_10");
            });

        AddCustomPersistence(services, configuration);

        return services;
    }

    [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
    private static void AddCustomPersistence(IServiceCollection services, IConfiguration configuration)
    {
        // Configure your own custom dependency injections here...
    }
}
