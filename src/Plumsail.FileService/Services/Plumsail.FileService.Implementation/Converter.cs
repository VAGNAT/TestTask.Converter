using Plumsail.FileData.Interfaces;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using Microsoft.Extensions.Logging;

namespace Plumsail.FileService.Implementation
{
    /// <summary>
    /// Implementation of the <see cref="IConverter"/> interface for converting HTML to PDF using Puppeteer.
    /// </summary>
    public class Converter : IConverter
    {
        /// <inheritdoc />
        public async Task<byte[]> HtmlToPdfAsync(byte[] data)
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, Timeout = 0 });
            await using var page = await browser.NewPageAsync();
            page.DefaultTimeout = 0;

            var htmlContent = System.Text.Encoding.UTF8.GetString(data);

            await page.SetContentAsync(htmlContent);

            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.Letter,
                Landscape = true,
                PrintBackground = true
            };

            return await page.PdfDataAsync(pdfOptions);
        }
    }
}
