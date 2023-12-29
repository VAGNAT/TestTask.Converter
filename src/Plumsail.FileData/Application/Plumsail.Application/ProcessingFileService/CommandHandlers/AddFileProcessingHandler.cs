using AutoMapper;
using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Commands;
using Plumsail.FileData.Application.Repositories.Abstractions;
using Plumsail.FileData.Domain.Entities;
using Plumsail.FileData.Domain.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.CommandHandlers
{
    /// <summary>
    /// Handler for processing the addition of a file.
    /// </summary>
    public sealed class AddFileProcessingHandler : IRequestHandler<AddFileProcessingCommandAsync, FileProcessingDto>
    {
        private readonly IFileProcessingRepository _fileRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddFileProcessingHandler"/> class.
        /// </summary>
        /// <param name="fileRepository">The repository for file processing operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        public AddFileProcessingHandler(IFileProcessingRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the asynchronous request to add a file.
        /// </summary>
        /// <param name="request">The command to add a file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The DTO representing the newly added file processing.</returns>
        public async Task<FileProcessingDto> Handle(AddFileProcessingCommandAsync request, CancellationToken cancellationToken)
        {
            var newFile = _fileRepository.Add(_mapper.Map<FileProcessing>(request.File));

            await _fileRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FileProcessingDto>(newFile);
        }
    }
}
