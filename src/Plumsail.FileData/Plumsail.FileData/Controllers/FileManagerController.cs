using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plumsail.FileData.Application.ProcessingFileService.Commands;
using Plumsail.FileData.Application.ProcessingFileService.Queries;
using Plumsail.FileData.Domain.Entities;
using Plumsail.FileData.Domain.EntitiesDto;
using Plumsail.FileData.Helpers;
using Plumsail.FileData.Models;
using Plumsail.FileData.ResponseModels;

namespace Plumsail.FileData.Controllers
{
    /// <summary>
    /// API controller for managing file.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FileManagerController : Controller
    {
        private const string FILE_FORMAT = "text/html";

        private readonly IMapper _mapper;
        private readonly ISender _sender;

        /// <summary>
        /// Initializes a new instance of the FileManagerController class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="sender">The sender for MediatR requests.</param>
        public FileManagerController(IMapper mapper, ISender sender)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Uninitialized property");
            _sender = sender ?? throw new ArgumentNullException(nameof(sender), "Uninitialized property");
        }

        /// <summary>
        /// Retrieves a list of files.
        /// </summary>
        /// <returns>A list of files.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<FileProcessingResponseShort>), 200)]
        public async Task<IActionResult> GetSessionFilesById()
        {
            var sessionId = CookieHandler.GetSessionIdFromCookies(HttpContext);

            return Ok(_mapper.Map<IEnumerable<FileProcessingResponseShort>>(await _sender.Send(new GetSessionFilesProcessingByIdQueryAsync(sessionId))));
        }

        /// <summary>
        /// Returns the file at the specified id.
        /// </summary>
        /// <returns>A file.</returns>
        [HttpGet("{id:Guid}", Name = "DownloadFileById")]
        [ProducesResponseType(typeof(File), 200)]
        [ProducesResponseType(typeof(FileProcessingResponseShort), 202)]
        public async Task<IActionResult> DownloadFileById([FromRoute] Guid id)
        {

            var file = await _sender.Send(new GetFileProcessingByIdQueryAsync(id));

            if (file.Status)
            {
                return File(file.OutputFile!, "application/pdf", file.InputFile);
            }

            return AcceptedAtRoute(nameof(DownloadFileById));
        }

        /// <summary>
        /// Adds a new file.
        /// </summary>
        /// <returns>The id of the new file.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 201)]
        public async Task<ActionResult> SaveFile(IFormFile file)
        {
            if (file.ContentType != FILE_FORMAT)
            {
                return BadRequest("Invalid file format. Only uploading files with the HTML extension is possible.");
            }

            NewFileModel newFile = new() { SessionId = CookieHandler.GetSessionIdFromCookies(HttpContext), Name = file.FileName };
            var addFile = await _sender.Send(new AddFileProcessingCommandAsync(_mapper.Map<FileProcessingDto>(newFile)));

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);

                var sendTask = _sender.Send(new SendFileProcessingForConversionToPdfCommandAsync(addFile.Id, ms.ToArray()));
            }
            return CreatedAtAction(nameof(SaveFile), new { addFile.SessionId, addFile.InputFile }, addFile.Id);
        }

        /// <summary>
        /// Deletes a file by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the post to delete.</param>
        /// <returns>A message confirming the deletion.</returns>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            await _sender.Send(new DeleteFileProcessingCommandAsync(id));

            return Ok($"File with id = {id} has been removed");
        }
    }
}
