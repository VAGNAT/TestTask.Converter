namespace Plumsail.FileData.Models
{
    /// <summary>
    /// Represents a model for creating a new file.
    /// </summary>
    public sealed class NewFileModel
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        public required Guid SessionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public required string Name { get; set; }
    }
}
