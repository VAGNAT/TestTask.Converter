namespace Plumsail.Converter.Middleware
{
    /// <summary>
    /// Extension methods for adding logging middleware to the application pipeline.
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds logging middleware to the application pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to configure the application's request pipeline.</param>
        public static void UseLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
