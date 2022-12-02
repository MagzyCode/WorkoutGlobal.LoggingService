using AutoMapper;
using MassTransit;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.Api.Repositories;
using WorkoutGlobal.LoggingService.API.Models;
using WorkoutGlobal.Shared.Messages;

namespace WorkoutGlobal.LoggingService.Api.Consumers
{
    /// <summary>
    /// Consumer of create log messages.
    /// </summary>
    public class CreateLogConsumer : IConsumer<CreateLogMessage>
    {
        /// <summary>
        /// Ctor for create log consumer.
        /// </summary>
        /// <param name="logRepository">Log repository instance.</param>
        /// <param name="severityRepository">Severity repository instance.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        public CreateLogConsumer(
            ILogRepository<Guid, Log> logRepository,
            ISeverityRepository<int, API.Models.Severity> severityRepository,
            IMapper mapper)
        {
            LogRepository = logRepository;
            SeverityRepository = severityRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Log repository.
        /// </summary>
        public ILogRepository<Guid, Log> LogRepository { get; set; }

        /// <summary>
        /// Severity repository.
        /// </summary>
        public ISeverityRepository<int, Severity> SeverityRepository { get; set; }

        /// <summary>
        /// AutoMapper instance.
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Consume messages.
        /// </summary>
        /// <param name="context">Message context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<CreateLogMessage> context)
        {
            var message = context.Message;

            var creationLog = Mapper.Map<Log>(message);

            creationLog.LogTime = DateTime.UtcNow;
            creationLog.SeverityId = await SeverityRepository.GetSeverityIdByName(message.Sevetity);

            await LogRepository.CreateLogAsync(creationLog);
        }
    }
}
