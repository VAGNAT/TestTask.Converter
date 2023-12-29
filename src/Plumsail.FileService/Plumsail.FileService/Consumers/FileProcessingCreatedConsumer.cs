using MassTransit;
using Plumsail.FileData.Interfaces;
using RabbitMQ.Events.Models;
using System.Diagnostics;
using System.Threading;

namespace Plumsail.FileService.Consumers
{
    /// <summary>
    /// Consumer for processing created file events.
    /// </summary>
    public sealed class FileProcessingCreatedConsumer : IConsumer<FileProcessingCreated>
    {
        private readonly IConverter _converter;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<FileProcessingCreatedConsumer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingCreatedConsumer"/> class.
        /// </summary>
        /// <param name="converter">The converter for converting HTML to PDF.</param>
        /// <param name="publishEndpoint">The MassTransit publish endpoint for publishing events.</param>
        /// <param name="logger">The logger for logging messages.</param>
        public FileProcessingCreatedConsumer(IConverter converter, IPublishEndpoint publishEndpoint, ILogger<FileProcessingCreatedConsumer> logger)
        {
            _converter = converter;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        /// <summary>
        /// Consumes the created file event.
        /// </summary>
        /// <param name="context">The consume context for the event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Consume(ConsumeContext<FileProcessingCreated> context)
        {
            try
            {
                _logger.LogInformation($"Consume fileId:{context.Message.Id}");
                _logger.LogInformation($"File processing start: {DateTime.Now} fileId:{context.Message.Id}");

                var fileBase = context.Message.InputFile;
                var filePdf = await _converter.HtmlToPdfAsync(fileBase);

                _logger.LogInformation($"File processing end: {DateTime.Now} fileId:{context.Message.Id}");

                await _publishEndpoint.Publish<FileProcessingUpdated>(new
                {
                    context.Message.Id,
                    OutputFile = filePdf
                });

                _logger.LogInformation($"Publish fileId:{context.Message.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(FileProcessingCreatedConsumer));
                throw;
            }

        }
    }
}
