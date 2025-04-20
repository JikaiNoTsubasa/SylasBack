using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace sylas_api.Global;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => 500
        };

        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Detail = exception.StackTrace,
            Status = statusCode,
            Type = exception.GetType().Name,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
