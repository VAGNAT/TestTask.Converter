namespace RabbitMQ.Events.Models
{
    /// <summary>
    /// Represents an event for file processing to be sent to RabbitMQ.
    /// </summary>
    public sealed class FileProcessingCreated
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the input file represented as a byte array.
        /// </summary>
        public required byte[] InputFile { get; set; }
    }
}
