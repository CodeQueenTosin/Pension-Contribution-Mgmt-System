using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using PensionContributionSystem.Application.Exceptions;

namespace PensionContributionSystem.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An error occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                BusinessRuleException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                DbUpdateException dbEx when IsDuplicateEmailException(dbEx) => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorMessage = exception switch
            {
                BusinessRuleException brEx => brEx.Message,
                ValidationException valEx => valEx.Message,
                KeyNotFoundException notFoundEx => notFoundEx.Message,
                DbUpdateException dbEx when IsDuplicateEmailException(dbEx) => "A member with this email already exists.",
                _ => "An unexpected error occurred. Please try again later."
            };

            context.Response.StatusCode = statusCode;

            var response = new { error = errorMessage };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private bool IsDuplicateEmailException(DbUpdateException ex)
        {
            return ex.InnerException?.Message.ToLower().Contains("duplicate") == true ||
                   ex.InnerException?.Message.ToLower().Contains("unique constraint") == true ||
                   ex.InnerException?.Message.Contains("IX_Members_Email") == true;
        }
    }
}
