using System.Net;
using System.Text.Json;

namespace TaskManagement.API.Middleware
{
    namespace TaskManagement.API.Middleware
    {
        public class ExceptionMiddleware
        {
            private readonly RequestDelegate _next;

            public ExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "application/json";

                    var statusCode = (int)HttpStatusCode.BadRequest;

                    var response = new
                    {
                        statusCode = statusCode,
                        message = ex.Message,
                        path = context.Request.Path.ToString(),
                        timestamp = DateTimeOffset.Now
                    };

                    context.Response.StatusCode = statusCode;

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            }
        }
    }
}
