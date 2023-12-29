using MassTransit;
using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Commands;
using Plumsail.FileData.Domain.EntitiesDto;
using RabbitMQ.Events.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.CommandHandlers
{
    /// <summary>
    /// Handler for processing the command to send a file for conversion to PDF.
    /// </summary>
    public sealed class SendFileProcessingForConversionToPdfHandler : IRequestHandler<SendFileProcessingForConversionToPdfCommandAsync>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendFileProcessingForConversionToPdfHandler"/> class.
        /// </summary>
        /// <param name="publishEndpoint">The MassTransit publish endpoint for sending messages.</param>
        public SendFileProcessingForConversionToPdfHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        /// <summary>
        /// Handles the asynchronous command to send a file for conversion to PDF.
        /// </summary>
        /// <param name="request">The command to send a file for conversion to PDF.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task Handle(SendFileProcessingForConversionToPdfCommandAsync request, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish<FileProcessingCreated>(new
            {
                Id = request.FileId,
                InputFile = request.File
            }, cancellationToken);
        }
    }
}
