using Newtonsoft.Json;
using Plumsail.FileData.Helpers;
using System.Net;

namespace Plumsail.FileData.Middleware
{
    /// <summary>
    /// Middleware for checking the presence of session cookies.
    /// </summary>
    public class CheckCookiesMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckCookiesMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware delegate in the pipeline.</param>
        public CheckCookiesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to check the presence of session cookies.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            CookieHandler.GetSessionIdFromCookies(context);

            await _next.Invoke(context);
        }
    }
}
