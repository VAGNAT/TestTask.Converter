using Serilog;

namespace Plumsail.Converter
{
    /// <summary>
    /// Utility class for registering services.
    /// </summary>
    public static class Registrar
    {
        /// <summary>
        /// Configures logging settings based on the application environment.
        /// </summary>
        public static void ConfigureLogging()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                    $"appsettings.{environment}.json", optional: true
                ).Build();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.Console()
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
