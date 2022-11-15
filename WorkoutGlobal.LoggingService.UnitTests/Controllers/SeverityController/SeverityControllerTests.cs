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
    public class SeverityControllerTests
    {
        public SeverityControllerTests()
        {
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

            #region Creation validator mock

            CreationValidatorMock
               .Setup(x => x.ValidateAsync(It.IsAny<CreationSeverityDto>(), default))
               .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure() } });

            #endregion

            #region Updation validator mock

            UpdationValidatorMock
               .Setup(x => x.ValidateAsync(It.IsAny<UpdationSeverityDto>(), default))
               .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure() } });

            #endregion

            #region Mapper mock

            MapperMock
                .Setup(x => x.Map<SeverityDto>(It.IsAny<API.Models.Severity>()))
                .Returns(new SeverityDto());

            MapperMock
                .Setup(x => x.Map<IEnumerable<SeverityDto>>(It.IsAny<IEnumerable<API.Models.Severity>>()))
                .Returns(new List<SeverityDto>());

            MapperMock
                .Setup(x => x.Map<API.Models.Severity>(It.IsAny<CreationSeverityDto>()))
                .Returns(new API.Models.Severity());

            MapperMock
                .Setup(x => x.Map<API.Models.Severity>(It.IsAny<UpdationSeverityDto>()))
                .Returns(new API.Models.Severity());

            MapperMock
                .Setup(x => x.Map<IEnumerable<LogDto>>(It.IsAny<IEnumerable<Log>>()))
                .Returns(new List<LogDto>());

            #endregion

            SeverityController = new(
                severityRepository: SeverityRepositoryMock.Object,
                creationValidator: CreationValidatorMock.Object,
                updationValidator: UpdationValidatorMock.Object,
                mapper: MapperMock.Object);
        }

        public SeverityController SeverityController { get; set; }

        public Mock<ISeverityRepository<int, API.Models.Severity>> SeverityRepositoryMock { get; set; } = new();

        public Mock<IValidator<CreationSeverityDto>> CreationValidatorMock { get; set; } = new();

        public Mock<IValidator<UpdationSeverityDto>> UpdationValidatorMock { get; set; } = new();

        public Mock<IMapper> MapperMock { get; set; } = new();

        public Fixture Fixture { get; set; } = new();

        [Fact]
        public async Task GetSeverity_EmptyParam_ReturnBadRequest()
        {
            // arrange
            var id = 0;

            // act 
            var result = await SeverityController.GetSeverity(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id isn't valid.");
            error.Details.Should().Be("Searchable severity cannot be found because id isn't valid.");
        }

        [Fact]
        public async Task GetSeverity_ModelNotExists_ReturnNotFound()
        {
            // arrange
            var id = 1;

            SeverityRepositoryMock.Setup(x => x.GetSeverityAsync(id));

            // act 
            var result = await SeverityController.GetSeverity(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Severity not found.");
            error.Details.Should().Be("Cannot find severity with given id.");
        }

        [Fact]
        public async Task GetSeverity_ValidState_ReturnOk()
        {
            // arrange
            var id = 1;

            // act 
            var result = await SeverityController.GetSeverity(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<SeverityDto>();
        }

        [Fact]
        public async Task GetAllSeverities_ValidState_ReturnOk()
        {
            // arrange
            // act 
            var result = await SeverityController.GetAllSeverities();

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<List<SeverityDto>>();
        }

        [Fact]
        public async Task CreateSeverity_ValidationNotPass_ReturnBadRequest()
        {
            // arrange
            var creationSeverityDto = new CreationSeverityDto();

            // act 
            var result = await SeverityController.CreateSeverity(creationSeverityDto);

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
        public async Task CreateSeverity_ValidState_ReturnCreated()
        {
            // arrange
            var creationSeverityDto = new CreationSeverityDto();

            CreationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<CreationSeverityDto>(), default))
                .ReturnsAsync(new ValidationResult());

            // act 
            var result = await SeverityController.CreateSeverity(creationSeverityDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();

            var createdResult = result.As<CreatedResult>();
            createdResult.Value.Should().NotBeNull();
            createdResult.Value.Should().BeOfType<int>();
            createdResult.Location.Should().NotBeNullOrEmpty();
            createdResult.Location.Should().BeOfType<string>();
            createdResult.Location.Should().Contain($"api/severities/");
        }

        [Fact]
        public async Task DeleteSeverity_InvalidParam_ReturnBadRequest()
        {
            // arrange
            var id = 0;

            // act 
            var result = await SeverityController.DeleteSeverity(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id isn't valid.");
            error.Details.Should().Be("Searchable severity cannot be found because id isn't valid.");
        }

        [Fact]
        public async Task DeleteSeverity_ModelNotExists_ReturnNotFound()
        {
            // arrange
            var id = 1;

            SeverityRepositoryMock.Setup(x => x.GetSeverityAsync(id));

            // act 
            var result = await SeverityController.DeleteSeverity(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Severity not found.");
            error.Details.Should().Be("Cannot find severity with given id.");
        }

        [Fact]
        public async Task DeleteSeverity_ValidState_ReturnNoContent()
        {
            // arrange
            var id = 1;

            // act 
            var result = await SeverityController.DeleteSeverity(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateSeverity_ValidationError_ReturnBadRequest()
        {
            // arrange
            var id = 0;

            // act
            var result = await SeverityController.UpdateSeverity(id, null);

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
        public async Task UpdateSeverity_InvalidId_ReturnBadRequest()
        {
            // arrange
            var id = 0;

            UpdationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdationSeverityDto>(), default))
                .ReturnsAsync(new ValidationResult());

            // act
            var result = await SeverityController.UpdateSeverity(id, null);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id isn't valid.");
            error.Details.Should().Be("Searchable severity cannot be found because id isn't valid.");
        }

        [Fact]
        public async Task UpdateLog_ModelNotExists_ReturnNotFound()
        {
            // arrange
            var id = 1;

            UpdationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdationSeverityDto>(), default))
                .ReturnsAsync(new ValidationResult());

            SeverityRepositoryMock
                .Setup(x => x.GetSeverityAsync(id));

            // act
            var result = await SeverityController.UpdateSeverity(id, null);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Severity not found.");
            error.Details.Should().Be("Cannot find severity with given id.");
        }

        [Fact]
        public async Task UpdateLog_ValidState_ReturnOk()
        {
            // arrange
            var id = 1;
            var updationSeverityDto = Fixture.Create<UpdationSeverityDto>();

            UpdationValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdationSeverityDto>(), default))
                .ReturnsAsync(new ValidationResult());

            // act
            var result = await SeverityController.UpdateSeverity(id, updationSeverityDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetSeverityLogs_InvalidId_ReturnBadRequest()
        {
            // arrange
            var id = 0;

            // act
            var result = await SeverityController.GetSeverityLogs(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id isn't valid.");
            error.Details.Should().Be("Searchable severity cannot be found because id isn't valid.");
        }

        [Fact]
        public async Task GetSeverityLogs_ModelNotFound_ReturnNotFound()
        {
            // arrange
            var id = 1;

            SeverityRepositoryMock
                .Setup(x => x.GetSeverityAsync(id));

            // act
            var result = await SeverityController.GetSeverityLogs(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var badRequestResult = result.As<NotFoundObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Severity not found.");
            error.Details.Should().Be("Cannot find severity with given id.");
        }

        [Fact]
        public async Task GetSeverityLogs_ValidState_ReturnOk()
        {
            // arrange
            var id = 1;

            // act
            var result = await SeverityController.GetSeverityLogs(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<List<LogDto>>();
        }
    }
}
