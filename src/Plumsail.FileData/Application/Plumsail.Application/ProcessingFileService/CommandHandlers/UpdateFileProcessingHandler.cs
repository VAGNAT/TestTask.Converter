using AutoMapper;
using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Commands;
using Plumsail.FileData.Application.Repositories.Abstractions;
using Plumsail.FileData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.CommandHandlers
{
    /// <summary>
    /// Handler for processing the command to update a file processing entry.
    /// </summary>
    public sealed class UpdateFileProcessingHandler : IRequestHandler<UpdateFileProcessingCommandAsync>
    {
        private readonly IFileProcessingRepository _fileRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateFileProcessingHandler"/> class.
        /// </summary>
        /// <param name="fileRepository">The repository for file processing operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        public UpdateFileProcessingHandler(IFileProcessingRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the asynchronous command to update a file processing entry.
        /// </summary>
        /// <param name="request">The command to update a file processing entry.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public async Task Handle(UpdateFileProcessingCommandAsync request, CancellationToken cancellationToken)
        {
            var existFile = await _fileRepository.GetByIdAsync(request.File.Id) ??
                throw new KeyNotFoundException($"Not found {nameof(FileProcessing)} with this id: {request.File.Id}");            
            existFile.OutputFile = request.File.OutputFile;
            existFile.Status = true;

            _fileRepository.Update(_mapper.Map<FileProcessing>(existFile));
            await _fileRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
