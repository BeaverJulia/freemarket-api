using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Shop.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var problemDetails = exception switch
        {
            CartNotFoundException => new ProblemDetails
            {
                Title = "Cart not found",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.NotFound
            },
            ProductNotFoundException => new ProblemDetails
            {
                Title = "Product not found",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.NotFound
            },
            InvalidDiscountCodeException => new ProblemDetails
            {
                Title = "Invalid discount code",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.BadRequest
            },
            ArgumentException => new ProblemDetails
            {
                Title = "Invalid request",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.BadRequest
            },
            _ => new ProblemDetails
            {
                Title = "An error occurred",
                Detail = "An unexpected error occurred while processing your request",
                Status = (int)HttpStatusCode.InternalServerError
            }
        };

        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
