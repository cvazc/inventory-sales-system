using System.Net;
using InventorySalesSystem.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace InventorySalesSystem.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing request.");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problem = new ProblemDetails
        {
            Title = "An error occurred while processing your request.",
            Type = "https://httpstatuses.com/500",
            Status = (int)HttpStatusCode.InternalServerError
        };

        switch (exception)
        {
            case BadRequestException:
                problem.Status = (int)HttpStatusCode.BadRequest;
                problem.Type = "https://httpstatuses.com/400";
                problem.Title = exception.Message;
                break;

            case NotFoundException:
                problem.Status = (int)HttpStatusCode.NotFound;
                problem.Type = "https://httpstatuses.com/404";
                problem.Title = exception.Message;
                break;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsJsonAsync(problem);
    }
}