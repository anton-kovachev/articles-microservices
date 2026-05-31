using Blocks.Domain;
using Blocks.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;

namespace Blocks.AspNetCore.Middlewares;

public sealed class GlobalExceptionMiddleware(RequestDelegate _next, ILogger<GlobalExceptionMiddleware> _logger)
{
    public static HttpStatusCode MapStatusCode(Exception ex) => ex switch
    {
        ValidationException => HttpStatusCode.BadRequest,
        BadRequestException => HttpStatusCode.BadRequest,
        NotFoundException => HttpStatusCode.NotFound,
        DomainException => HttpStatusCode.BadRequest,
        ArgumentException => HttpStatusCode.BadRequest,
        _ => HttpStatusCode.InternalServerError
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch(OperationCanceledException ex)
        {
            if(context.Response.HasStarted)
                context.Response.StatusCode = 499;
        }
        catch(Exception  ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = (int)MapStatusCode(exception);
        if (statusCode == (int)HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception. TraceId={TraceId}", context.TraceIdentifier);
        else
            _logger.LogDebug(exception, "Unhandled exception. TraceId={TraceId}", context.TraceIdentifier);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            context.Response.StatusCode,
            exception.Message,
            Details = exception.StackTrace
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context,  ValidationException exception, HttpStatusCode statusCode)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var validationErrors = exception.Errors.Select(err => new
        {
            err.PropertyName,
            err.ErrorMessage
        });

        var response = new
        {
            context.Response.StatusCode,
            exception.Message,
            Details = exception.StackTrace,
            Erros = validationErrors
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}
