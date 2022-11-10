using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.API.Models;
using WorkoutGlobal.LoggingService.Api.Repositories.ModelRepositories;
using FluentValidation;

namespace WorkoutGlobal.LoggingService.Api.Controllers
{
    /// <summary>
    /// Represents severity controller.
    /// </summary>
    [Route("api/severities")]
    [ApiController]
    [Produces("application/json")]
    public class SeverityController : ControllerBase
    {
        private ISeverityRepository _severityRepository;
        private IMapper _mapper;
        private IValidator<CreationSeverityDto> _creationValidator;
        private IValidator<UpdationSeverityDto> _updationValidator;

        /// <summary>
        /// Ctor for severityRepository.
        /// </summary>
        /// <param name="severityRepository">Severity repository instanse.</param>
        /// <param name="mapper">AutoMapper instanse.</param>
        /// <param name="creationValidator">Creation validator instanse.</param>
        /// <param name="updationValidator">Updation validator instanse.</param>
        public SeverityController(
            ISeverityRepository severityRepository,
            IMapper mapper,
            IValidator<CreationSeverityDto> creationValidator,
            IValidator<UpdationSeverityDto> updationValidator)
        {
            SeverityRepository = severityRepository;
            Mapper = mapper;
            UpdationValidator = updationValidator;
            CreationValidator = creationValidator;
        }

        /// <summary>
        /// Severity repository.
        /// </summary>
        public ISeverityRepository SeverityRepository
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
        /// Creation validator.
        /// </summary>
        public IValidator<CreationSeverityDto> CreationValidator
        {
            get => _creationValidator;
            private set => _creationValidator = value;
        }

        /// <summary>
        /// Updation validator.
        /// </summary>
        public IValidator<UpdationSeverityDto> UpdationValidator
        {
            get => _updationValidator;
            private set => _updationValidator = value;
        }

        /// <summary>
        /// Get severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns>Return severity info.</returns>
        /// <response code="200">Severity was successfully get.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(type: typeof(SeverityDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSeverity(int id)
        {
            if (id <= 0)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id isn't valid.",
                    Details = "Searchable severity cannot be found because id isn't valid."
                });

            var severity = await SeverityRepository.GetSeverityAsync(id);

            if (severity is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Severity not found.",
                    Details = "Cannot find severity with given id."
                });

            var severityDto = Mapper.Map<SeverityDto>(severity);

            return Ok(severityDto);
        }

        /// <summary>
        /// Get all severities.
        /// </summary>
        /// <returns>Returns all severities.</returns>
        /// <response code="200">Severities was successfully get.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet]
        [ProducesResponseType(type: typeof(IEnumerable<SeverityDto>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSeverities()
        {
            var severities = await SeverityRepository.GetAllSeveritiesAsync();

            var severitiesDto = Mapper.Map<IEnumerable<SeverityDto>>(severities);

            return Ok(severitiesDto);
        }

        /// <summary>
        /// Create severity.
        /// </summary>
        /// <param name="creationSeverityDto">Creation model.</param>
        /// <returns>Returns location of created severity.</returns>
        /// <response code="201">Severity was successfully created.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPost]
        [ProducesResponseType(type: typeof(CreatedResult), statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSeverity([FromBody] CreationSeverityDto creationSeverityDto)
        {
            var validationResult = await CreationValidator.ValidateAsync(creationSeverityDto);

            if (!validationResult.IsValid)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Incoming creation DTO model isn't valid.",
                    Details = validationResult.ToString()
                });

            var creationSeverity = Mapper.Map<API.Models.Severity>(creationSeverityDto);

            var createdId = await SeverityRepository.CreateSeverityAsync(creationSeverity);

            return Created($"api/severities/{createdId}", createdId);
        }

        /// <summary>
        /// Delete severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns></returns>
        /// <response code="204">Severity was successfully deleted.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(type: typeof(NoContentResult), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSeverity(int id)
        {
            if (id <= 0)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id isn't valid.",
                    Details = "Searchable severity cannot be found because id isn't valid."
                });

            var deletingSeverity = await SeverityRepository.GetSeverityAsync(id);

            if (deletingSeverity is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Severity not found.",
                    Details = "Cannot find severity with given id."
                });

            await SeverityRepository.DeleteSeverityAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Update severity.
        /// </summary>
        /// <param name="id">severity id.</param>
        /// <param name="updationSeverityDto">Updation model.</param>
        /// <returns></returns>
        /// <response code="204">Severity was successfully updated.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(type: typeof(NoContentResult), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSeverity(int id, [FromBody] UpdationSeverityDto updationSeverityDto)
        {
            var validationResult = await UpdationValidator.ValidateAsync(updationSeverityDto);

            if (!validationResult.IsValid)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Incoming updation DTO model isn't valid.",
                    Details = validationResult.ToString()
                });

            if (id <= 0)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id isn't valid.",
                    Details = "Searchable severity cannot be found because id isn't valid."
                });

            var severity = await SeverityRepository.GetSeverityAsync(id, false);

            if (severity is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Severity not found.",
                    Details = "Cannot find severity with given id."
                });

            var updationSeverity = Mapper.Map<API.Models.Severity>(updationSeverityDto);
            updationSeverity.Id = id;

            await SeverityRepository.UpdateSeverityAsync(updationSeverity);

            return NoContent();
        }

        /// <summary>
        /// Get severity level logs.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns>Returns severity logs.</returns>
        /// <response code="200">Severity logs was successfully get.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet("{id}/logs")]
        [ProducesResponseType(type: typeof(IEnumerable<LogDto>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSeverityLogs(int id)
        {
            if (id <= 0)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id isn't valid.",
                    Details = "Searchable severity cannot be found because id isn't valid."
                });

            var severity = await SeverityRepository.GetSeverityAsync(id);

            if (severity is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Severity not found.",
                    Details = "Cannot find severity with given id."
                });

            var logs = await SeverityRepository.GetAllSeverityLogs(id);

            var logsDto = Mapper.Map<IEnumerable<LogDto>>(logs);

            return Ok(logsDto);
        }
    }
}
