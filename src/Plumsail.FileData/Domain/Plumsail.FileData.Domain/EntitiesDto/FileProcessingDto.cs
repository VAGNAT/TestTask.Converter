using Plumsail.FileData.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Domain.EntitiesDto
{
    /// <summary>
    /// Represents a data transfer object (DTO) for file processing entities.
    /// </summary>
    public sealed class FileProcessingDto : BaseEntityDto
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        public required Guid SessionId { get; set; }

        /// <summary>
        /// Gets or sets the input file path.
        /// </summary>
        public required string InputFile { get; set; }

        /// <summary>
        /// Gets or sets the status of the file processing.
        /// </summary>
        public required bool Status { get; set; }

        /// <summary>
        /// Gets or sets the output file content.
        /// </summary>
        public byte[]? OutputFile { get; set; }
    }
}
