using Plumsail.Application.Repositories;
using Plumsail.FileData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Application.Repositories.Abstractions
{
    /// <summary>
    /// Repositiry interface to manage ProcessingFile entities.
    /// </summary>
    public interface IFileProcessingRepository : IRepository<FileProcessing>
    {
        /// <summary>
        /// Gets all file processing entries by session identifier asynchronously.
        /// </summary>
        /// <param name="sessionId">The session identifier to filter by.</param>
        /// <returns>A task representing the asynchronous operation, containing the collection of file processing entries.</returns>
        Task<IEnumerable<FileProcessing>> GetAllBySessionId(Guid sessionId);
    }
}
