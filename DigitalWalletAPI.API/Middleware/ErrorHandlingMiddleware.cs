using System.Net;
using System.Text.Json;
using DigitalWalletAPI.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DigitalWalletAPI.API.Middleware
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new
            {
                message = exception.Message,
                statusCode = exception is DomainException domainEx ? domainEx.StatusCode : 500
            };

            response.StatusCode = errorResponse.statusCode;

            if (exception is DomainException)
            {
                _logger.LogWarning(exception, "DomainException");
            }
            else
            {
                _logger.LogError(exception, "ErrorOver");
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }
} 