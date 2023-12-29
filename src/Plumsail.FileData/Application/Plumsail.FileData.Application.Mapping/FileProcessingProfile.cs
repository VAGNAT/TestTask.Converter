using AutoMapper;
using Plumsail.FileData.Domain.Entities;
using Plumsail.FileData.Domain.EntitiesDto;

namespace Plumsail.FileData.Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="FileProcessing"/> and <see cref="FileProcessingDto"/>.
    /// </summary>
    public sealed class FileProcessingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingProfile"/> class.
        /// </summary>
        public FileProcessingProfile()
        {
            CreateMap<FileProcessing, FileProcessingDto>();
            CreateMap<FileProcessingDto, FileProcessing>();
        }
    }
}
