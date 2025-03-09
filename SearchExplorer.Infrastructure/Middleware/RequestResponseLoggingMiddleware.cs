using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace SearchExplorer.Infrastructure.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            _logger.LogInformation($"[{timestamp}] Incoming request: {context.Request.Method} {context.Request.Path}");

            var originalBodyStream = context.Response.Body;
            await _next(context);
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                _logger.LogInformation($"[{timestamp}] Outgoing response: {context.Response.StatusCode}");

                responseBody.Seek(0, SeekOrigin.Begin);
                string responseBodyContent = await new StreamReader(responseBody).ReadToEndAsync();

                _logger.LogInformation($"[{timestamp}] Response Body: {responseBodyContent}");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

    }
}
