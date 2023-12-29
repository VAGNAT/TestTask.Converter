using AutoMapper;
using Plumsail.FileData.Domain.EntitiesDto;
using Plumsail.FileData.Models;
using Plumsail.FileData.ResponseModels;
using RabbitMQ.Events.Models;

namespace Plumsail.FileData.Mapping
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="FileProcessingDto"/> and UI-related models.
    /// </summary>
    public class FileProcessingUiProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingUiProfile"/> class.
        /// </summary>
        public FileProcessingUiProfile()
        {
            CreateMap<FileProcessingDto, FileProcessingResponseShort>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.InputFile))
                .ForCtorParam(nameof(FileProcessingResponseShort.Name), options => options.MapFrom(source => source.InputFile))
                .ForCtorParam(nameof(FileProcessingResponseShort.Id), options => options.MapFrom(source => source.Id));

            CreateMap<NewFileModel, FileProcessingDto>()
                .ForMember(dest => dest.Id, map => map.Ignore())
                .ForMember(dest => dest.OutputFile, map => map.Ignore())
                .ForMember(dest => dest.Status, act => act.MapFrom(src => false))
                .ForMember(dest => dest.InputFile, act => act.MapFrom(src => src.Name));

            CreateMap<FileProcessingUpdated, FileProcessingDto>()
                .ForMember(dest => dest.InputFile, map => map.Ignore())
                .ForMember(dest => dest.SessionId, map => map.Ignore());
        }
    }
}
