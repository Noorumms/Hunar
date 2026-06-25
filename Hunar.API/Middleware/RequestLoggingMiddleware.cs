using Microsoft.AspNetCore.Hosting.Builder;

namespace Hunar.API.Middleware
{
    // Logs every incoming request with timestamp and response time
    // This is how you monitor what's hitting your API in production
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Record when request arrived
            var startTime = DateTime.UtcNow;

            // Log the incoming request
            _logger.LogInformation(
                "[{Timestamp}] --> {Method} {Path}",
                startTime.ToString("HH:mm:ss"),
                context.Request.Method,
                context.Request.Path
            );

            // Pass to next middleware
            await _next(context);

            // Calculate how long it took
            var duration = DateTime.UtcNow - startTime;

            // Log the response
            _logger.LogInformation(
                "[{Timestamp}] <-- {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms",
                DateTime.UtcNow.ToString("HH:mm:ss"),
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                duration.TotalMilliseconds.ToString("F0")
            );
        }
    }

    //public static class RequestLoggingMiddlewareExtensions
    //{
    //    public  IApplicationBuilder UseRequestLogger(
    //        this IApplicationBuilder app)
    //    {

    //        return app.UseMiddleware<RequestLoggingMiddleware>();
    //    }
    //}

}