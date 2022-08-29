using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.WebApi.REST.LoggingMiddleware", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api.Middleware;

public class LoggingMiddleware
{
    const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    private readonly RequestDelegate _next;
    private readonly ILogger _logger = Log.ForContext<LoggingMiddleware>();

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _next(httpContext);
            sw.Stop();

            var statusCode = httpContext.Response?.StatusCode;
            var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

            if (httpContext.Items.ContainsKey("Exception"))
            {
                var exception = (Exception)httpContext.Items["Exception"];
                LogException(httpContext, sw, exception);
            }
            else
            {
                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : _logger;
                log.Write(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds);
            }
        }
        catch (Exception ex)
        {
            LogException(httpContext, sw, ex);
        }
    }

    private static bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
    {
        sw.Stop();

        LogForErrorContext(httpContext)
            .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, sw.Elapsed.TotalMilliseconds);

        return false;
    }

    private static ILogger LogForErrorContext(HttpContext httpContext)
    {
        var request = httpContext.Request;

        var result = Log
            .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
            .ForContext("RequestHost", request.Host)
            .ForContext("RequestProtocol", request.Protocol);

        if (request.HasFormContentType)
            result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

        return result;
    }
}
