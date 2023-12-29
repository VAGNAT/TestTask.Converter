namespace Plumsail.FileData.Interfaces
{
    /// <summary>
    /// Interface for a converter that converts HTML to PDF.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Converts HTML data to PDF asynchronously.
        /// </summary>
        /// <param name="data">The HTML data to convert.</param>
        /// <returns>A task representing the asynchronous operation, containing the converted PDF data.</returns>
        Task<byte[]> HtmlToPdfAsync(byte[] data);
    }
}
