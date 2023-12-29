namespace Plumsail.FileData.ResponseModels
{
    /// <summary>
    /// Represents a short response for file processing.
    /// </summary>
    internal sealed record FileProcessingResponseShort(
        Guid Id,
        string Name,
        bool Status);    
}
