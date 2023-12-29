using AutoMapper;
using MediatR;
using Plumsail.FileData.Application.ProcessingFileService.Queries;
using Plumsail.FileData.Application.Repositories.Abstractions;
using Plumsail.FileData.Domain.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.QueriesHandlers
{
    /// <summary>
    /// Handler for processing the query to retrieve file processing entries by session identifier.
    /// </summary>
    public sealed class GetSessionFilesProcessingHandler : IRequestHandler<GetSessionFilesProcessingByIdQueryAsync, IEnumerable<FileProcessingDto>>
    {
        private readonly IFileProcessingRepository _fileRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSessionFilesProcessingHandler"/> class.
        /// </summary>
        /// <param name="fileRepository">The repository for file processing operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        public GetSessionFilesProcessingHandler(IFileProcessingRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the asynchronous query to retrieve file processing entries by session identifier.
        /// </summary>
        /// <param name="request">The query to retrieve file processing entries by session identifier.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The collection of DTOs representing the retrieved file processing entries.</returns>
        public async Task<IEnumerable<FileProcessingDto>> Handle(GetSessionFilesProcessingByIdQueryAsync request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<FileProcessingDto>>(await _fileRepository.GetAllBySessionId(request.SessionId));
        }
    }
}
