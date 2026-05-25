using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Middleware
{
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;

            if (exception.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                status = HttpStatusCode.NotFound;
            else if (exception.Message.Contains("invalid", StringComparison.OrdinalIgnoreCase))
                status = HttpStatusCode.BadRequest;
            else if (exception.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase))
                status = HttpStatusCode.Unauthorized;
            else if (exception.Message.Contains("exists", StringComparison.OrdinalIgnoreCase))
                status = HttpStatusCode.Conflict; // 409 for duplicate resources
            else
                status = HttpStatusCode.InternalServerError;

            var response = new
            {
                status = (int)status,
                error = status.ToString(),
                message = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
