using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using AspNetCoreRateLimit;
using CoreApplicationFilterVal_10.Api.Middleware;
using CoreApplicationFilterVal_10.Application;
using CoreApplicationFilterVal_10.Application.Common.Mapping;
using CoreApplicationFilterVal_10.Domain;
using CoreApplicationFilterVal_10.Domain.Common.Exceptions;
using CoreApplicationFilterVal_10.Domain.Common.Mapping;
using CoreApplicationFilterVal_10.Domain.Common.Validation;
using CoreApplicationFilterVal_10.Persistence;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.WebApi.REST.Program", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api;

public class Program
{
    private static readonly Dictionary<string, string> _details = new();
    private static readonly string _corsPolicy = "DefaultCorsPolicy";


    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Starting up");

        try
        {
            // CREATE BUILDER
            var builder = WebApplication.CreateBuilder(args);
            _details.Add("Environment", builder.Environment.EnvironmentName ?? "Local");
            _details.Add("Version", Assembly.GetExecutingAssembly().GetName().Version?.ToString());
            _details.Add("Platform", RuntimeInformation.OSDescription);

            ConfigureFramework(builder.Services);
            ConfigureSettings(builder.Configuration);
            ConfigureLogging(builder.Host, builder.Configuration, builder.Environment, "CoreApplicationFilterVal_10");
            ConfigureSwagger(builder.Services);
            ConfigureHealthChecks(builder.Services, builder.Configuration);
            ConfigureRateLimiting(builder.Services, builder.Configuration);
            ConfigureApplication(builder.Services, builder.Configuration);
            ConfigureDomain(builder.Services, builder.Configuration);
            ConfigurePersistence(builder.Services, builder.Configuration);
            ConfigureMapping(builder.Services);
            ConfigureCORS(builder.Services, builder.Configuration);

            var app = builder.Build();

            EnableSecurityHeaders(app, builder.Configuration);
            EnableMiddleware(app);
            EnableSwagger(app);
            EnableHealthChecks(app);

            app.UseIpRateLimiting();
            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.Information("Shut down complete");
            Log.CloseAndFlush();
        }
    }

    static void ConfigureFramework(IServiceCollection services)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    if (!context.ModelState.IsValid)
                    {
                        throw new ValidationException("One or more validation errors", context.ModelState.Select(ms => new ValidationError
                        {
                            Field = ms.Key,
                            ErrorMessage = ms.Value?.Errors.First().ErrorMessage
                        }).ToList());
                    }
                    return new BadRequestResult();
                };
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.MaxDepth = 8;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
    }

    static void ConfigureSettings(ConfigurationManager configuration)
    {
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{_details["Environment"]}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        configuration.AddJsonFile("secrets.json", true, false);
        configuration.AddKeyPerFile("/app/secrets", true);
    }

    static void ConfigureLogging(IHostBuilder host, IConfiguration configuration, IHostEnvironment env, String applicationName)
    {
        host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration));

    }

    static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.DescribeAllParametersInCamelCase();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CoreApplicationFilterVal_10",
                Version = "v1",
                Description = string.Join("<br />", _details.Select(kv => $"{kv.Key}: <strong>{kv.Value}</strong>"))
            });
            c.EnableAnnotations();

            // Add custom types
            c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
            c.MapType<decimal?>(() => new OpenApiSchema { Type = "number", Format = "decimal?" });
            c.MapType<object>(() => new OpenApiSchema { Type = "object" });

            // Change operation id to method name
            c.CustomOperationIds(e =>
                e.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null
            );
        });
    }

    static void ConfigureHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        var healthCheckBuilder = services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

    }

    static void ConfigureRateLimiting(IServiceCollection services, IConfiguration configuration)
    {
        //Configured on per project basis
        services.AddOptions();
        services.AddMemoryCache();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddHttpContextAccessor();

        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimitOptions"));
    }

    private static void ConfigureApplication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication(configuration);
    }

    private static void ConfigureDomain(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomain(configuration);
    }

    private static void ConfigurePersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
    }

    private static void ConfigureMapping(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddAutoMapper(typeof(AppMappingProfile));
    }

    private static void ConfigureCORS(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
            {
                options.AddPolicy(name: _corsPolicy, builder =>
                {
                    builder.WithOrigins(GetAllowedHosts(configuration))
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
    }

    private static string[] GetAllowedHosts(IConfiguration configuration)
    {
        var allowedHosts = configuration.GetSection("AllowedHosts").Get<string[]>();
        // Add configuration guard
        if (allowedHosts == null || !allowedHosts.Any())
        {
            allowedHosts = new[] { "*" };
        }
        return allowedHosts;
    }

    private static void EnableSecurityHeaders(IApplicationBuilder app, IConfiguration configuration)
    {
        app.Use(async (context, next) =>
            {
            // Defines approved sources of content that the browser may load
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';img-src  data: https: 'self';object-src 'none'; script-src 'self' 'unsafe-inline';style-src 'self' 'unsafe-inline'; upgrade-insecure-requests;");
            // Controls how much referrer information (sent via the Referer header) should be included with requests
                context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            // Ensures the site can't be iframed. If you are using iframes on the same domain, you can change the value to SAMEORIGIN.
                context.Response.Headers.Add("X-Frame-Options", "DENY");
            // Causes browsers to stop loading the page when a cross-site scripting attack is identified
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            // MIME-type sniffing is an attack where a hacker tries to exploit missing metadata on served files.
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            // Indicates whether the response can be shared with requesting code from the given origin.
                context.Response.Headers.Add("Access-Control-Allow-Origin", GetAllowedHosts(configuration));
            // Tells the browser which platform features the website needs (i.e. accelerometer=(), camera=(), microphone=())
                context.Response.Headers.Add("Permissions-Policy", "");
            // Disable the possibility of Flash making cross-site requests
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                await next();
            });
    }
    static void EnableMiddleware(IApplicationBuilder app)
    {
        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();
    }

    static void EnableSwagger(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "CoreApplicationFilterVal_10";
            c.EnableDeepLinking();
            c.DisplayOperationId();
            c.DisplayRequestDuration();
            c.DefaultModelExpandDepth(2);
            c.DefaultModelsExpandDepth(-1);
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

        });
    }

    static void EnableHealthChecks(IApplicationBuilder app)
    {
        app.UseHealthChecks("/health/self", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self"),
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("dependencies"),
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
    }
}