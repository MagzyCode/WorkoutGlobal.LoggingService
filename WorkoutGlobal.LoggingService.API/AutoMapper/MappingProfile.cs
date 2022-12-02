using AutoMapper;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.API.Models;
using WorkoutGlobal.Shared.Messages;

namespace WorkoutGlobal.LoggingService.Api.AutoMapper
{
    /// <summary>
    /// Class for configure mapping rules via AutoMapper library.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Ctor for set mapping rules for models and DTOs.
        /// </summary>
        public MappingProfile()
        {
            #region Logs

            CreateMap<Log, LogDto>();
            CreateMap<CreationLogDto, Log>();
            CreateMap<UpdationLogDto, Log>();
            CreateMap<CreateLogMessage, Log>();

            #endregion

            #region Severities

            CreateMap<Severity, SeverityDto>();
            CreateMap<CreationSeverityDto, Severity>();
            CreateMap<UpdationSeverityDto, Severity>();

            #endregion
        }
    }
}
