using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PotteryService.Application.Common.Exceptions;

namespace PotteryService.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        IHostEnvironment hostEnvironment,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException exception)
        {
            _logger.LogWarning(exception, "Validation failure while processing request.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (KeyNotFoundException exception)
        {
            _logger.LogWarning(exception, "Requested resource was not found.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (ConflictException exception)
        {
            _logger.LogWarning(exception, "Business conflict while processing request.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status409Conflict, exception.Message);
        }
        catch (DbUpdateException exception)
        {
            _logger.LogWarning(exception, "Database update conflict while processing request.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status409Conflict, "Database constraint conflict.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception while processing request.");
            await WriteProblemDetailsAsync(
                context,
                StatusCodes.Status500InternalServerError,
                _hostEnvironment.IsDevelopment() ? exception.Message : "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemDetailsAsync(HttpContext context, int statusCode, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode switch
            {
                StatusCodes.Status400BadRequest => "Du lieu khong hop le",
                StatusCodes.Status404NotFound => "Khong tim thay du lieu",
                StatusCodes.Status409Conflict => "Du lieu da ton tai",
                _ => "Loi he thong"
            },
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
