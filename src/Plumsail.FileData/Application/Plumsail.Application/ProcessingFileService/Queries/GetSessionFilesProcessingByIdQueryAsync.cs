using MediatR;
using Plumsail.FileData.Domain.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.Queries
{
    /// <summary>
    /// Represents a query to retrieve file processing entries by session identifier.
    /// </summary>
    public record GetSessionFilesProcessingByIdQueryAsync(Guid SessionId) : IRequest<IEnumerable<FileProcessingDto>>;
}
