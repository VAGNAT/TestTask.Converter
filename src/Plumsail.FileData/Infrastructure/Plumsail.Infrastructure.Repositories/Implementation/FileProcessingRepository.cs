using Microsoft.EntityFrameworkCore;
using Plumsail.FileData.Application.Interfaces;
using Plumsail.FileData.Application.Repositories.Abstractions;
using Plumsail.FileData.Domain.Entities;
using Plumsail.FileData.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plumsail.FileData.Infrastructure.Repositories.Implementation
{
    /// <summary>
    /// Repository for managing ProcessingFile entities.
    /// </summary>
    public sealed class FileProcessingRepository : BaseRepository<FileProcessing>, IFileProcessingRepository
    {
        /// <summary>
        /// Initializes a new instance of the ProcessingFile class.
        /// </summary>
        /// <param name="context">The application context.</param>
        public FileProcessingRepository(IApplicationContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all file processing entries by session identifier asynchronously.
        /// </summary>
        /// <param name="sessionId">The session identifier to filter by.</param>
        /// <returns>A task representing the asynchronous operation, containing the collection of file processing entries.</returns>
        public async Task<IEnumerable<FileProcessing>> GetAllBySessionId(Guid sessionId)
        {
            return await _entitySet.AsNoTracking().Where(x => x.SessionId == sessionId).ToListAsync();
        }
    }
}
