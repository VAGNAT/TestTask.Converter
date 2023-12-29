using AutoMapper;

using Plumsail.FileData.Application.Mapping;
using Plumsail.FileData.Application.Repositories.Abstractions;
using Plumsail.FileData.Infrastructure.Repositories.Implementation;
using Plumsail.FileData.Infrastructure;
using Plumsail.FileData.Mapping;
using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Queries;
using Plumsail.FileData.Application.ProcessingFileService.QueriesHandlers;
using Plumsail.FileData.Domain.EntitiesDto;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Plumsail.FileData.Application.ProcessingFileService.Commands;
using Plumsail.FileData.Application.ProcessingFileService.CommandHandlers;
using MassTransit;
using Plumsail.FileData.Consumers;
using RabbitMQ.Events.Models;
using Serilog;

namespace Plumsail.FileData
{
    /// <summary>
    /// Utility class for registering services and configuring logging within the application.
    /// </summary>
    public static class Registrar
    {
        /// <summary>
        /// Adds services to the service collection, including MediatR handlers, repositories, and RabbitMQ configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configuration">The configuration for the application.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton((IConfigurationRoot)configuration)
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly))
                .AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()))
                .AddInfrastructureServices(configuration)
                .InstallHandlers()
                .InstallRepositories()
                .InstallRabbitMQ(configuration);
        }

        /// <summary>
        /// Installs MediatR request handlers.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to install handlers to.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        private static IServiceCollection InstallHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection
            //ProcessingFile
            .AddTransient<IRequestHandler<GetSessionFilesProcessingByIdQueryAsync, IEnumerable<FileProcessingDto>>, GetSessionFilesProcessingHandler>()
            .AddTransient<IRequestHandler<GetFileProcessingByIdQueryAsync, FileProcessingDto>, GetFileProcessingHandler>()
            .AddTransient<IRequestHandler<AddFileProcessingCommandAsync, FileProcessingDto>, AddFileProcessingHandler>()
            .AddTransient<IRequestHandler<DeleteFileProcessingCommandAsync, Guid>, DeleteFileProcessingHandler>()
            .AddTransient<IRequestHandler<SendFileProcessingForConversionToPdfCommandAsync>, SendFileProcessingForConversionToPdfHandler>()
            .AddTransient<IRequestHandler<UpdateFileProcessingCommandAsync>, UpdateFileProcessingHandler>();

            return serviceCollection;
        }

        /// <summary>
        /// Installs repositories.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to install repositories to.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        private static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<IFileProcessingRepository, FileProcessingRepository>();
            return serviceCollection;
        }

        /// <summary>
        /// Installs RabbitMQ configuration for MassTransit.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to install RabbitMQ configuration to.</param>
        /// <param name="configuration">The configuration for the application.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        private static IServiceCollection InstallRabbitMQ(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddMassTransit(x =>
                {
                    x.AddConsumersFromNamespaceContaining<FileProcessingUpdatedConsumer>();                    
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
        /// Gets the MapperConfiguration for AutoMapper.
        /// </summary>
        /// <returns>The configured <see cref="MapperConfiguration"/>.</returns>
        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                //ProcessingFile
                cfg.AddProfile<FileProcessingProfile>();
                cfg.AddProfile<FileProcessingUiProfile>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }

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
