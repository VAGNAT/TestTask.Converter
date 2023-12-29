using AutoMapper;
using MassTransit;
using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Commands;
using Plumsail.FileData.Domain.EntitiesDto;
using RabbitMQ.Events.Models;

namespace Plumsail.FileData.Consumers
{
    /// <summary>
    /// Consumer for processing updated file events.
    /// </summary>
    public sealed class FileProcessingUpdatedConsumer : IConsumer<FileProcessingUpdated>
    {
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly ILogger<FileProcessingUpdatedConsumer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingUpdatedConsumer"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        /// <param name="sender">The MassTransit sender for sending commands.</param>
        /// <param name="logger">The logger for logging messages.</param>
        public FileProcessingUpdatedConsumer(IMapper mapper, ISender sender, ILogger<FileProcessingUpdatedConsumer> logger)
        {
            _mapper = mapper;
            _sender = sender;
            _logger = logger;
        }

        /// <summary>
        /// Consumes the updated file event.
        /// </summary>
        /// <param name="context">The consume context for the event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Consume(ConsumeContext<FileProcessingUpdated> context)
        {
            _logger.LogInformation($"Consume file for updating. fileId: {context.Message.Id}");
                        
            await _sender.Send(new UpdateFileProcessingCommandAsync(_mapper.Map<FileProcessingDto>(context.Message)));
        }
    }
}
