using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Commands;
using Plumsail.FileData.Application.Repositories.Abstractions;
using Plumsail.FileData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.CommandHandlers
{
    /// <summary>
    /// Handler for processing the deletion of a file.
    /// </summary>
    public sealed class DeleteFileProcessingHandler : IRequestHandler<DeleteFileProcessingCommandAsync, Guid>
    {
        private readonly IFileProcessingRepository _fileRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFileProcessingHandler"/> class.
        /// </summary>
        /// <param name="fileRepository">The repository for file processing operations.</param>
        public DeleteFileProcessingHandler(IFileProcessingRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// Handles the asynchronous request to delete a file.
        /// </summary>
        /// <param name="request">The command to delete a file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The identifier of the deleted file.</returns>
        public async Task<Guid> Handle(DeleteFileProcessingCommandAsync request, CancellationToken cancellationToken)
        {
            var existFile = await _fileRepository.GetByIdAsync(request.Id) ??
                throw new KeyNotFoundException($"Not found {nameof(FileProcessing)} with this id: {request.Id}");

            _fileRepository.Delete(existFile);
            await _fileRepository.SaveChangesAsync(cancellationToken);

            return request.Id;
        }
    }
}
