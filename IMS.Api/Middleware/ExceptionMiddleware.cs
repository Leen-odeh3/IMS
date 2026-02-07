namespace IMS.Api.Middleware;
using global::IMS.Core.Exceptions;
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Something went wrong";

        switch (ex)
        {
            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = ex.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = ex.Message;
                break;
        }

        var response = new
        {
            statusCode = (int)statusCode,
            message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

