using System;
using System.Data;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Api.Common.Responses;
using CoreApplicationFilterVal_10.Domain.Common.Exceptions;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Http;
using Serilog;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.WebApi.REST.ExceptionMiddleware", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
    };

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (NotFoundException ex)
        {
            await HandleNotFoundExceptionAsync(httpContext, ex);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(httpContext, ex);
        }
        catch (DataException ex)
        {
            httpContext.Items.Add("Exception", ex);
            await HandleDataExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            httpContext.Items.Add("Exception", ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var responsePayload = JsonSerializer.Serialize(
            new ErrorResponse
            {
                StatusCode = 500,
                Message = exception.Message,
                DisplayMessage = "There was a problem while processing your request."
            }, SerializerOptions);

        return context.Response.WriteAsync(responsePayload);
    }

    private static Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        var responsePayload = JsonSerializer.Serialize(
            new NotFoundResponse
            {
                StatusCode = 404,
                Message = $"NotFound: '{exception.Identifier}' or type '{exception.Entity}'",
                DisplayMessage = $"The '{exception.Entity}' you are looking could not be found."
            }, SerializerOptions);

        return context.Response.WriteAsync(responsePayload);
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var responsePayload = JsonSerializer.Serialize(
            new ValidationErrorResponse
            {
                StatusCode = 400,
                Message = exception.Message,
                DisplayMessage = "One or more validation errors occurred while processing your request.",
                Errors = exception.Errors

            }, SerializerOptions);

        return context.Response.WriteAsync(responsePayload);
    }

    private static Task HandleDataExceptionAsync(HttpContext context, DataException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var responsePayload = JsonSerializer.Serialize(
            new ErrorResponse
            {
                StatusCode = 500,
                Message = exception.Message,
                DisplayMessage = "There was a problem while processing your request."
            }, SerializerOptions);

        return context.Response.WriteAsync(responsePayload);
    }


}
