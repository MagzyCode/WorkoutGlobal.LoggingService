using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Controllers
{
    /// <summary>
    /// Represents log controller.
    /// </summary>
    [Route("api/logs")]
    [ApiController]
    [Produces("application/json")]
    public class LogController : ControllerBase
    {
        private ILogRepository<Guid, Log> _logRepository;
        private ISeverityRepository<int, API.Models.Severity> _severityRepository;
        private IMapper _mapper;
        private IValidator<CreationLogDto> _creationLogValidator;
        private IValidator<UpdationLogDto> _updationLogValidator;

        /// <summary>
        /// Ctor for log controller.
        /// </summary>
        /// <param name="logRepository">Log repository.</param>
        /// <param name="severityRepository">Severity repository.</param>
        /// <param name="creationValidator">Creation log model validator.</param>
        /// <param name="updationValidator">Updation log model validator.</param>
        /// <param name="mapper">AutoMapper instanse.</param>
        public LogController(
            ILogRepository<Guid, Log> logRepository,
            ISeverityRepository<int, API.Models.Severity> severityRepository,
            IValidator<CreationLogDto> creationValidator,
            IValidator<UpdationLogDto> updationValidator,
            IMapper mapper)
        {
            LogRepository = logRepository;
            SeverityRepository = severityRepository;
            CreationValidator = creationValidator;
            UpdationValidator = updationValidator;
            Mapper = mapper;
        }

        /// <summary>
        /// Log repository.
        /// </summary>
        public ILogRepository<Guid, Log> LogRepository
        {
            get => _logRepository;
            private set => _logRepository = value;
        }

        /// <summary>
        /// Repository manager.
        /// </summary>
        public ISeverityRepository<int, API.Models.Severity> SeverityRepository
        {
            get => _severityRepository;
            private set => _severityRepository = value;
        }

        /// <summary>
        /// Auto mapping helper.
        /// </summary>
        public IMapper Mapper
        {
            get => _mapper;
            private set => _mapper = value;
        }

        /// <summary>
        /// Creation model validator.
        /// </summary>
        public IValidator<CreationLogDto> CreationValidator
        {
            get => _creationLogValidator;
            private set => _creationLogValidator = value;
        }

        /// <summary>
        /// Updation model validator.
        /// </summary>
        public IValidator<UpdationLogDto> UpdationValidator
        {
            get => _updationLogValidator;
            private set => _updationLogValidator = value;
        }

        /// <summary>
        /// Get log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns>Return log info.</returns>
        /// <response code="200">Log was successfully get.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(type: typeof(LogDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLog(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable log cannot be found because id is empty."
                });

            var log = await LogRepository.GetLogAsync(id);

            if (log is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Log not found.",
                    Details = "Cannot find log with given id."
                });

            var logDto = Mapper.Map<LogDto>(log);
            logDto.SeverityName = await SeverityRepository.GetSeverityNameById(log.SeverityId);

            return Ok(logDto);
        }

        /// <summary>
        /// Get all logs.
        /// </summary>
        /// <returns>Returns all logs.</returns>
        /// <response code="200">Logs was successfully get.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet]
        [ProducesResponseType(type: typeof(IEnumerable<LogDto>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await LogRepository.GetAllLogsAsync();

            var logsDto = Mapper.Map<IEnumerable<LogDto>>(logs);

            return Ok(logsDto);
        }

        /// <summary>
        /// Create log.
        /// </summary>
        /// <param name="creationLogDto">Creation model.</param>
        /// <returns>Returns location of created log.</returns>
        /// <response code="201">Log was successfully created.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPost]
        [ProducesResponseType(type: typeof(CreatedResult), statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLog([FromBody] CreationLogDto creationLogDto)
        {
            var validationResult = await CreationValidator.ValidateAsync(creationLogDto);

            if (!validationResult.IsValid)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Incoming creation DTO model isn't valid.",
                    Details = validationResult.ToString()
                });

            var creationLog = Mapper.Map<Log>(creationLogDto);

            creationLog.LogTime = DateTime.UtcNow;
            creationLog.SeverityId = await SeverityRepository.GetSeverityIdByName(creationLogDto.Severity);

            var createdId = await LogRepository.CreateLogAsync(creationLog);

            return Created($"api/logs/{createdId}", createdId);
        }

        /// <summary>
        /// Delete log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns></returns>
        /// <response code="204">Log was successfully deleted.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(type: typeof(NoContentResult), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLog(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable log cannot be found because id is empty."
                });

            var deletingLog = await LogRepository.GetLogAsync(id);

            if (deletingLog is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Log not found.",
                    Details = "Cannot find log with given id."
                });

            await LogRepository.DeleteLogAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Update log.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <param name="updationLogDto">Updation model.</param>
        /// <returns></returns>
        /// <response code="204">Log was successfully updated.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(type: typeof(NoContentResult), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLog(Guid id, [FromBody] UpdationLogDto updationLogDto)
        {
            var validationResult = await UpdationValidator.ValidateAsync(updationLogDto);

            if (!validationResult.IsValid)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Incoming updation DTO model isn't valid.",
                    Details = validationResult.ToString()
                });

            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable log cannot be found because id is empty."
                });

            var log = await LogRepository.GetLogAsync(id);

            if (log is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Log not found.",
                    Details = "Cannot find log with given id."
                });

            var updationLog = Mapper.Map<Log>(updationLogDto);
            updationLog.Id = id;

            var updationSeverityId = await SeverityRepository.GetSeverityIdByName(updationLogDto.Severity);

            if (updationSeverityId != log.SeverityId)
                updationLog.SeverityId = updationSeverityId;

            await LogRepository.UpdateLogAsync(updationLog);

            return NoContent();
        }

    }
}
