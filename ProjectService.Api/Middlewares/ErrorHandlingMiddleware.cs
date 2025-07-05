using System.Net;
using System.Text.Json;
using FluentValidation;
using ProjectService.Application.Common.Exceptions;

namespace ProjectService.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing request");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var message = ex.Message;
        IEnumerable<string> errors = Array.Empty<string>();
        if (ex.GetType() == typeof(ValidationException))
        {
            var exception = (ValidationException)ex;
            errors = exception.Errors.Select(e => e.ErrorMessage);
            message = "Validation failed";
            code = HttpStatusCode.BadRequest;
        }
        else if (ex.GetType() == typeof(ForbiddenException))
        {
            message = ex.Message;
            code = HttpStatusCode.Forbidden;
        }
        else if (ex.GetType() == typeof(UnauthorizedAccessException))
        {
            message = ex.Message;
            code = HttpStatusCode.Unauthorized;
        }
        else if (ex.GetType() == typeof(NotFoundException))
        {
            message = ex.Message;
            code = HttpStatusCode.NotFound;
        }
        else if  (ex.GetType() == typeof(BadRequestException))
        {
            message = ex.Message;
            code = HttpStatusCode.BadRequest;
        }
        else if (ex.GetType() == typeof(InternalServerException))
        {
            message = ex.Message;
            code = HttpStatusCode.InternalServerError;
        }

        var result = JsonSerializer.Serialize(new { message, errors });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}