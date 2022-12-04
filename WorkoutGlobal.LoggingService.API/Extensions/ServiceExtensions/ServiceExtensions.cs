using FluentValidation;
using MassTransit;
using WorkoutGlobal.LoggingService.Api.Consumers;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.Api.Enums;
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

        /// <summary>
        /// Configure MassTransit.
        /// </summary>
        /// <param name="services">Project services.</param>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="broker">Message broker type.</param>
        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration, Broker broker)
        {
            services.AddMassTransit(options =>
            {
                options.AddConsumer<CreateLogConsumer>();

                switch (broker)
                {
                    case Broker.RabbitMQ:
                        options.UsingRabbitMq((context, configurator) =>
                        {
                            configurator.Host(configuration["MassTransitSettings:Hosts:RabbitMQHost"]);
                            configurator.ReceiveEndpoint(configuration["MassTransitSettings:Exchanges:CreateLog"], endpoint =>
                            {
                                endpoint.ConfigureConsumer<CreateLogConsumer>(context);
                            });
                        });
                        break;
                    case Broker.AzureServiceBus:
                        options.UsingAzureServiceBus((context, configurator) =>
                        {
                            configurator.Host(configuration["MassTransitSettings:Hosts:AzureSBHost"]);
                            configurator.ReceiveEndpoint(configuration["MassTransitSettings:Exchanges:CreateLog"], endpoint =>
                            {
                                endpoint.ConfigureConsumer<CreateLogConsumer>(context);
                            });
                        });
                        break;
                }
            });
        }
    }
}
