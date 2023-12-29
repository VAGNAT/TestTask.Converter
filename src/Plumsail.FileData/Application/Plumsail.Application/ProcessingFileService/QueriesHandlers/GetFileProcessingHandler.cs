using AutoMapper;
using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Queries;
using Plumsail.FileData.Application.Repositories.Abstractions;
using Plumsail.FileData.Domain.Entities;
using Plumsail.FileData.Domain.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.QueriesHandlers
{
    /// <summary>
    /// Handler for processing the query to retrieve a file processing entry by its identifier.
    /// </summary>
    public sealed class GetFileProcessingHandler : IRequestHandler<GetFileProcessingByIdQueryAsync, FileProcessingDto>
    {
        private readonly IFileProcessingRepository _fileRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFileProcessingHandler"/> class.
        /// </summary>
        /// <param name="fileRepository">The repository for file processing operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        public GetFileProcessingHandler(IFileProcessingRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the asynchronous query to retrieve a file processing entry by its identifier.
        /// </summary>
        /// <param name="request">The query to retrieve a file processing entry.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The DTO representing the retrieved file processing entry.</returns>
        public async Task<FileProcessingDto> Handle(GetFileProcessingByIdQueryAsync request, CancellationToken cancellationToken)
        {
            var existFile = await _fileRepository.GetByIdAsync(request.Id) ??
                throw new KeyNotFoundException($"Not found {nameof(FileProcessing)} with this id: {request.Id}");

            return _mapper.Map<FileProcessingDto>(existFile);
        }
    }
}
