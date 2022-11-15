using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.Api.Controllers;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.UnitTests.Controllers
{
    public class LogControllerTests
    {
        public LogControllerTests()
        {
            #region Log repository mock

            LogRepositoryMock
                .Setup(x => x.GetLogAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Log());

            LogRepositoryMock
                .Setup(x => x.GetAllLogsAsync())
                .ReturnsAsync(new List<Log>());

            LogRepositoryMock
                .Setup(x => x.CreateLogAsync(It.IsAny<Log>()))
                .ReturnsAsync(Guid.NewGuid());

            LogRepositoryMock
                .Setup(x => x.DeleteLogAsync(It.IsAny<Guid>()));

            LogRepositoryMock
                .Setup(x => x.UpdateLogAsync(It.IsAny<Log>()));

            LogRepositoryMock
                .Setup(x => x.GetLogSevetiry(It.IsAny<Guid>()))
                .ReturnsAsync(new API.Models.Severity());

            #endregion

            #region Severity repository mock

            SeverityRepositoryMock
                .Setup(x => x.GetSeverityAsync(It.IsAny<int>()))
                .ReturnsAsync(new API.Models.Severity());

            SeverityRepositoryMock
                .Setup(x => x.GetAllSeveritiesAsync())
                .ReturnsAsync(new List<API.Models.Severity>());

            SeverityRepositoryMock
                .Setup(x => x.CreateSeverityAsync(It.IsAny<API.Models.Severity>()))
                .ReturnsAsync(1);

            SeverityRepositoryMock
               .Setup(x => x.DeleteSeverityAsync(It.IsAny<int>()));

            SeverityRepositoryMock
               .Setup(x => x.UpdateSeverityAsync(It.IsAny<API.Models.Severity>()));

            SeverityRepositoryMock
                .Setup(x => x.GetAllSeverityLogsAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Log>());

            SeverityRepositoryMock
                .Setup(x => x.GetSeverityNameById(It.IsAny<int>()))
                .ReturnsAsync("result");

            SeverityRepositoryMock
                .Setup(x => x.GetSeverityIdByName(It.IsAny<string>()))
                .ReturnsAsync(1);

            #endregion

            #region Mapper mock

            MapperMock
                .Setup(x => x.Map<LogDto>(It.IsAny<Log>()))
                .Returns(new LogDto());

            MapperMock
                .Setup(x => x.Map<IEnumerable<LogDto>>(It.IsAny<IEnumerable<Log>>()))
                .Returns(new List<LogDto>());

            MapperMock
                .Setup(x => x.Map<Log>(It.IsAny<CreationLogDto>()))
                .Returns(new Log());

            MapperMock
                .Setup(x => x.Map<Log>(It.IsAny<UpdationLogDto>()))
                .Returns(new Log());

            #endregion

            #region Creation validator mock

            CreationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<CreationLogDto>(), default))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure() } });

            #endregion

            #region Updation validator mock 

            UpdationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdationLogDto>(), default))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure() } });

            #endregion

            LogController = new(
                logRepository: LogRepositoryMock.Object,
                severityRepository: SeverityRepositoryMock.Object,
                creationValidator: CreationValidatorMock.Object,
                updationValidator: UpdationValidatorMock.Object,
                mapper: MapperMock.Object);
        }

        public LogController LogController { get; set; }

        public Mock<ILogRepository<Guid, Log>> LogRepositoryMock { get; set; } = new();

        public Mock<ISeverityRepository<int, API.Models.Severity>> SeverityRepositoryMock { get; set; } = new();

        public Mock<IValidator<CreationLogDto>> CreationValidatorMock { get; set; } = new();

        public Mock<IValidator<UpdationLogDto>> UpdationValidatorMock { get; set; } = new();

        public Mock<IMapper> MapperMock { get; set; } = new();

        public Fixture Fixture { get; set; } = new();

        [Fact]
        public async Task GetLog_EmptyParam_ReturnBadRequest()
        {
            // arrange
            var id = Guid.Empty;

            // act 
            var result = await LogController.GetLog(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id is empty.");
            error.Details.Should().Be("Searchable log cannot be found because id is empty.");
        }

        [Fact]
        public async Task GetLog_ModelNotExists_ReturnNotFound()
        {
            // arrange
            var id = Guid.NewGuid();

            LogRepositoryMock.Setup(x => x.GetLogAsync(id));

            // act 
            var result = await LogController.GetLog(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Log not found.");
            error.Details.Should().Be("Cannot find log with given id.");
        }

        [Fact]
        public async Task GetLog_ValidState_ReturnOk()
        {
            // arrange
            var id = Guid.NewGuid();

            // act 
            var result = await LogController.GetLog(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<LogDto>();
        }

        [Fact]
        public async Task GetAllLogs_ValidState_ReturnOk()
        {
            // arrange
            // act 
            var result = await LogController.GetAllLogs();

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<List<LogDto>>();
        }

        [Fact]
        public async Task CreateLog_ValidationNotPass_ReturnBadRequest()
        {
            // arrange
            var creationLogDto = new CreationLogDto();

            // act 
            var result = await LogController.CreateLog(creationLogDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Incoming creation DTO model isn't valid.");
        }

        [Fact]
        public async Task CreateLog_ValidState_ReturnCreated()
        {
            // arrange
            var creationLogDto = new CreationLogDto();

            CreationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<CreationLogDto>(), default))
                .ReturnsAsync(new ValidationResult());

            // act 
            var result = await LogController.CreateLog(creationLogDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();

            var createdResult = result.As<CreatedResult>();
            createdResult.Value.Should().NotBeNull();
            createdResult.Value.Should().BeOfType<Guid>();
            createdResult.Location.Should().NotBeNullOrEmpty();
            createdResult.Location.Should().BeOfType<string>();
            createdResult.Location.Should().Contain($"api/logs/");
        }

        [Fact]
        public async Task DeleteLog_EmptyParam_ReturnBadRequest()
        {
            // arrange
            var id = Guid.Empty;

            // act 
            var result = await LogController.DeleteLog(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id is empty.");
            error.Details.Should().Be("Searchable log cannot be found because id is empty.");
        }

        [Fact]
        public async Task DeleteLog_ModelNotExists_ReturnNotFound()
        {
            // arrange
            var id = Guid.NewGuid();

            LogRepositoryMock.Setup(x => x.GetLogAsync(id));

            // act 
            var result = await LogController.DeleteLog(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Log not found.");
            error.Details.Should().Be("Cannot find log with given id.");
        }

        [Fact]
        public async Task DeleteLog_ValidState_ReturnNoContent()
        {
            // arrange
            var id = Guid.NewGuid();

            // act 
            var result = await LogController.DeleteLog(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateLog_ValidationError_ReturnBadRequest()
        {
            // arrange
            var id = Guid.Empty;

            // act
            var result = await LogController.UpdateLog(id, null);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Incoming updation DTO model isn't valid.");
        }

        [Fact]
        public async Task UpdateLog_EmptyId_ReturnBadRequest()
        {
            // arrange
            var id = Guid.Empty;

            UpdationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdationLogDto>(), default))
                .ReturnsAsync(new ValidationResult());

            // act
            var result = await LogController.UpdateLog(id, null);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id is empty.");
            error.Details.Should().Be("Searchable log cannot be found because id is empty.");
        }

        [Fact]
        public async Task UpdateLog_ModelNotExists_ReturnNotFound()
        {
            // arrange
            var id = Guid.NewGuid();

            UpdationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdationLogDto>(), default))
                .ReturnsAsync(new ValidationResult());

            LogRepositoryMock
                .Setup(x => x.GetLogAsync(id));

            // act
            var result = await LogController.UpdateLog(id, null);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Log not found.");
            error.Details.Should().Be("Cannot find log with given id.");
        }

        [Fact]
        public async Task UpdateLog_ValidState_ReturnOk()
        {
            // arrange
            var id = Guid.NewGuid();
            var updationLogDto = Fixture.Create<UpdationLogDto>();

            UpdationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdationLogDto>(), default))
                .ReturnsAsync(new ValidationResult());

            // act
            var result = await LogController.UpdateLog(id, updationLogDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
