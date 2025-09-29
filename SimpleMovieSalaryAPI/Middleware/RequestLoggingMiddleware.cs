using SimpleMovieSalaryAPI.Interfaces;
using System.Diagnostics;

namespace SimpleMovieSalaryAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var request = context.Request;
            _logger.LogInfo($"Incoming Request: {request.Method} {request.Path}");

            // Call the next middleware
            await _next(context);

            stopwatch.Stop();
            var response = context.Response;
            _logger.LogInfo($"Outgoing Response: {response.StatusCode} handled in {stopwatch.ElapsedMilliseconds}ms");
        }
    }

}

