using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using OrderApi.Exceptions;

namespace OrderApi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        string message;
        switch (exception)
        {
            case NotFoundException notFoundEx:
                statusCode = StatusCodes.Status404NotFound;
                message = notFoundEx.Message;
                break; 
            case BadRequestException badReqEx:
                statusCode = StatusCodes.Status400BadRequest;
                message = badReqEx.Message;
                break;
            case ModelvalidationException modelValidateEx:
                statusCode = StatusCodes.Status400BadRequest;
                message = modelValidateEx.Message;
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = exception.Message ?? "Lỗi hệ thống, vui lòng thử lại sau.";
                break;
        }

        _logger.LogError(exception, "Error {StatusCode}: {Message}, TraceId: {TraceId}",
            statusCode, message, context.TraceIdentifier);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var errorResponse = new
        {
            code = statusCode,
            message,
            trace = context.TraceIdentifier
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
