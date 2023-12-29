using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.Commands
{
    /// <summary>
    /// Represents a command to delete a file processing entry by its identifier.
    /// </summary>
    public record DeleteFileProcessingCommandAsync(Guid Id) : IRequest<Guid>;
}
