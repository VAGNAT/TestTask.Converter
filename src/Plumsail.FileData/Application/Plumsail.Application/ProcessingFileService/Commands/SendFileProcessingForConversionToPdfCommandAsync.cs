using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.ProcessingFileService.Commands
{
    /// <summary>
    /// Represents a command to send a file for conversion to PDF.
    /// </summary>
    public record SendFileProcessingForConversionToPdfCommandAsync(Guid FileId, byte[] File) : IRequest;
}
