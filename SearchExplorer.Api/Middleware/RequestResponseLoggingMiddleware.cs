using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SearchExplorer.Api.Middleware
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

            // Log incoming request
            _logger.LogInformation(JsonSerializer.Serialize(new
            {
                Timestamp = timestamp,
                RequestMethod = context.Request.Method,
                RequestPath = context.Request.Path,
                QueryParams = context.Request.Query
            }));

            // Capture the response
            var originalBodyStream = context.Response.Body;
            using (var responseBodyStream = new MemoryStream())
            {
                context.Response.Body = responseBodyStream;

                await _next(context); // Process the request

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(responseBodyStream).ReadToEndAsync();

                // Log outgoing response in JSON format
                _logger.LogInformation(JsonSerializer.Serialize(new
                {
                    Timestamp = timestamp,
                    StatusCode = context.Response.StatusCode,
                    ResponseBody = TryParseJson(responseBodyText)
                }, new JsonSerializerOptions { WriteIndented = true }));

                // Restore the response body stream
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
        }

        private object TryParseJson(string jsonString)
        {
            try
            {
                return JsonSerializer.Deserialize<object>(jsonString);
            }
            catch
            {
                return jsonString; // Return raw text if it's not JSON
            }
        }
    }
}
