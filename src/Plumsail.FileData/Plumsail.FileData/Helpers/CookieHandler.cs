namespace Plumsail.FileData.Helpers
{
    /// <summary>
    /// Static class for handling cookies.
    /// </summary>
    public static class CookieHandler
    {
        /// <summary>
        /// Gets the session identifier from cookies in the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP context containing the cookies.</param>
        /// <returns>The session identifier retrieved from cookies.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the session has not been initialized.</exception>
        public static Guid GetSessionIdFromCookies(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.TryGetValue("SessionId", out string? session) && Guid.TryParse(session, out Guid sessionId))
            {
                return sessionId;
            }
            throw new InvalidOperationException("The session has not been initialized.");
        }
    }
}
