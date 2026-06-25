using System.Net;
using System.Text.Json;

namespace Hunar.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;


        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
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
                _logger.LogError(ex, "Aanunhandled exception occured ");
                await HandleExceptionAsync(context, ex);
            }

        }

        private static async Task HandleExceptionAsync(HttpContext context ,
            Exception exception)
        {
            var statusCode = exception switch
            {
                InvalidOperationException => HttpStatusCode.BadRequest,        // 400
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                ArgumentException => HttpStatusCode.BadRequest,

                _=> HttpStatusCode.InternalServerError

            };

            var response = new
            {
                statusCode = (int)statusCode,
                error = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);

        }










    }

    //public static class ExceptionMiddlewareExtensions
    //{
    //    public static async IApplicationBuilder UseExceptionHandler(
    //        this IApplicationBuilder app)
    //    {
    //        return app.UseMiddleware<ExceptionMiddleware>();
    //    }
    //}




}