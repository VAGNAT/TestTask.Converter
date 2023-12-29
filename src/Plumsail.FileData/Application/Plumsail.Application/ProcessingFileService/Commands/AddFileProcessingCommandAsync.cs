using MediatR;
using Plumsail.FileData.Domain.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.Commands
{
    /// <summary>
    /// Represents a command to add a file processing entry.
    /// </summary>
    public record AddFileProcessingCommandAsync(FileProcessingDto File) : IRequest<FileProcessingDto>;
}
