using Plumsail.FileData.Domain.Abstractions;

namespace Plumsail.FileData.Domain.Entities
{
    /// <summary>
    /// Represents a file processing entity.
    /// </summary>
    public class FileProcessing : BaseEntity
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
