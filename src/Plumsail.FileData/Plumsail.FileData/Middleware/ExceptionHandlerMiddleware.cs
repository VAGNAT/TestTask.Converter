using Newtonsoft.Json;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Plumsail.FileData.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions and providing appropriate HTTP responses.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware delegate in the pipeline.</param>
        /// <param name="logger">The logger for logging exceptions.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions and provide appropriate HTTP responses.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ExceptionHandlerMiddleware));
                HttpStatusCode code = ex switch
                {
                    KeyNotFoundException or FileNotFoundException => HttpStatusCode.NotFound,
                    UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                    ValidationException or InvalidOperationException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError,
                };
                await HandleException(context, ex, code).ConfigureAwait(false);
            }
        }

        private static Task HandleException(HttpContext context, Exception exception, HttpStatusCode statusCodeInput)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)statusCodeInput;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message + (exception.InnerException is not null ?
                    $" Inner exception: {exception.InnerException.Message}" : "")
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
