using MassTransit;
using MassTransit.Observables;
using Plumsail.FileData.Interfaces;
using Plumsail.FileService.Consumers;
using Plumsail.FileService.Implementation;
using RabbitMQ.Events.Models;
using Serilog;
using System;

namespace Plumsail.FileService
{
    /// <summary>
    /// Registrar class for configuring and adding services to the service collection.
    /// </summary>
    public static class Registrar
    {
        /// <summary>
        /// Adds services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The modified service collection.</returns>
        internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton((IConfigurationRoot)configuration)
                .InstallRabbitMQ(configuration)
                .InstallServices();
        }

        /// <summary>
        /// Installs RabbitMQ configuration.
        /// </summary>
        /// <param name="serviceCollection">The service collection to install RabbitMQ configuration.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The modified service collection.</returns>
        private static IServiceCollection InstallRabbitMQ(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddMassTransit(x =>
                {
                    x.AddConsumersFromNamespaceContaining<FileProcessingCreatedConsumer>();
                    x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(configuration["RabbitMq:Host"], "/", host =>
                            {
                                host.Username(configuration.GetValue("RabbitMq:Username", "guest"));
                                host.Password(configuration.GetValue("RabbitMq:Password", "guest"));
                            });
                            cfg.ConfigureEndpoints(context);
                        });

                });
            return serviceCollection;
        }

        /// <summary>
        /// Installs services in the service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection to install services into.</param>
        /// <returns>The modified service collection.</returns>
        private static IServiceCollection InstallServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IConverter, Converter>();

            return serviceCollection;
        }

        /// <summary>
        /// Configures logging for the application.
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
