using FluentValidation;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.Api.Repositories;
using WorkoutGlobal.LoggingService.Api.Validators;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.API.Extensions
{
    /// <summary>
    /// Base class for all service extensions.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure instances of repository classes.
        /// </summary>
        /// <param name="services">Project services.</param>
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILogRepository<Guid, Log>, LogRepository>();
            services.AddScoped<ISeverityRepository<int, Models.Severity>, SeverityRepository>();
        }

        /// <summary>
        /// Configure instances of validator classes.
        /// </summary>
        /// <param name="services">Project services.</param>
        public static void ConfigureValidators(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<CreationLogDto>, CreationLogDtoValidator>();
            services.AddSingleton<IValidator<UpdationLogDto>, UpdationLogDtoValidator>();
            services.AddSingleton<IValidator<CreationSeverityDto>, CreationSeverityDtoValidator>();
            services.AddSingleton<IValidator<UpdationSeverityDto>, UpdationSeverityDtoValidator>();
        }

    }
}
