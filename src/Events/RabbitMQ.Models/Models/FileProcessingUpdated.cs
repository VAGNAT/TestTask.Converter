namespace RabbitMQ.Events.Models
{
    /// <summary>
    /// Represents an event for updating file processing status and sending to RabbitMQ.
    /// </summary>
    public sealed class FileProcessingUpdated
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the status indicating the updated processing status.
        /// </summary>
        public required bool Status { get; set; }

        /// <summary>
        /// Gets or sets the output file represented as a byte array.
        /// </summary>
        public required byte[] OutputFile { get; set; }
    }
}
