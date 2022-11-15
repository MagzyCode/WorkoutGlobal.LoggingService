using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using System.Net;
using System.Net.Http.Json;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.IntegrationTests.Controllers
{
    public class LogControllerTests : IAsyncLifetime
    {
        public AppTestConnection<Guid> Connection { get; set; } = new();

        public Fixture Fixture { get; set; } = new();

        public CreationLogDto CreationModel { get; set; }

        public async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                Connection.PurgeList.Clear();

                CreationModel = Fixture.Build<CreationLogDto>()
                    .With(x => x.Message, "Test message")
                    .With(x => x.Severity, "Test name")
                    .Create();
            });   
        }

        public async Task DisposeAsync()
        {
            foreach (var id in Connection.PurgeList)
                await Connection.AppClient.DeleteAsync($"api/logs/purge/{id}");
        }

        [Fact]
        public async Task GetLog_ValidState_ReturnOk()
        {
            // arrange
            var url = "api/logs";
            var postSeverityResponse = await Connection.AppClient.PostAsJsonAsync("api/severities",
                new CreationSeverityDto()
                {
                    SeverityName = "Test name",
                    SeverityDescription = "Test description"
                });
            var createdSeverityId = await postSeverityResponse.Content.ReadFromJsonAsync<int>();

            var postResponse = await Connection.AppClient.PostAsJsonAsync("api/logs", CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<Guid>();

            // act
            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<LogDto>();

            await Connection.AppClient.DeleteAsync($"api/logs/purge/{createdId}");
            await Connection.AppClient.DeleteAsync($"api/severities/purge/{createdSeverityId}");

            // assert
            createdId.Should().NotBeEmpty();

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<LogDto>();
            createdModel.Message.Should().Be("Test message");
            createdModel.SeverityName.Should().Be("Test name");
        }

        [Fact]
        public async Task GetAllLogs_ValidState_ReturnOk()
        {
            // arrange
            var url = "api/logs";
            var postSeverityResponse = await Connection.AppClient.PostAsJsonAsync("api/severities",
                new CreationSeverityDto()
                {
                    SeverityName = "Test name",
                    SeverityDescription = "Test description"
                });
            var createdSeverityId = await postSeverityResponse.Content.ReadFromJsonAsync<int>();

            var postResponse = await Connection.AppClient.PostAsJsonAsync("api/logs", CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<Guid>();

            // act
            var getResponse = await Connection.AppClient.GetAsync(url);
            var createdModel = await getResponse.Content.ReadFromJsonAsync<List<LogDto>>();

            await Connection.AppClient.DeleteAsync($"api/logs/purge/{createdId}");
            await Connection.AppClient.DeleteAsync($"api/severities/purge/{createdSeverityId}");

            // assert
            createdId.Should().NotBeEmpty();

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<List<LogDto>>();
            createdModel.Count.Should().BeGreaterThanOrEqualTo(1);
        }


        [Fact]
        public async Task CreateLog_NullParam_ReturnBadRequest()
        {
            // arrange
            CreationModel = null;

            // act
            var postResponse = await Connection.AppClient.PostAsJsonAsync("api/logs", CreationModel);
            var error = await postResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            error.Should().NotBeNull();
            error.Should().BeOfType<ErrorDetails>();
            error.Message.Should().Be("Incoming creation DTO model is null.");
            error.Details.Should().Be("Incoming creation DTO model cannot be null.");
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateLog_InvalidParam_ReturnBadRequest()
        {
            // arrange
            CreationModel = Fixture.Build<CreationLogDto>()
                .With(x => x.Message, string.Empty)
                .With(x => x.Severity, string.Empty)
                .Create();

            var postSeverityResponse = await Connection.AppClient.PostAsJsonAsync("api/severities", 
                new CreationSeverityDto() 
                { 
                    SeverityName = "Test name", 
                    SeverityDescription = "Test description"
                });
            var createdSeverityId = await postSeverityResponse.Content.ReadFromJsonAsync<int>();

            // act
            var postResponse = await Connection.AppClient.PostAsJsonAsync("api/logs", CreationModel);
            var error = await postResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            await Connection.AppClient.DeleteAsync($"api/severities/purge/{createdSeverityId}");

            // assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            error.Should().NotBeNull();
            error.Should().BeOfType<ErrorDetails>();
            error.Message.Should().Be("Incoming creation DTO model isn't valid.");
            error.Details.Should().Be("'Message' должно быть заполнено.\r\n'Severity' должно быть заполнено.");
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateLog_ValidState_ReturnCreated()
        {
            // arrange
            var url = "api/logs";
            var postSeverityResponse = await Connection.AppClient.PostAsJsonAsync("api/severities",
                new CreationSeverityDto()
                {
                    SeverityName = "Test name",
                    SeverityDescription = "Test description"
                });
            var createdSeverityId = await postSeverityResponse.Content.ReadFromJsonAsync<int>();

            // act
            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<Guid>();

            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<LogDto>();

            await Connection.AppClient.DeleteAsync($"api/logs/purge/{createdId}");
            await Connection.AppClient.DeleteAsync($"api/severities/purge/{createdSeverityId}");

            // assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            createdId.Should().NotBeEmpty();

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<LogDto>();
            createdModel.Message.Should().Be("Test message");
            createdModel.SeverityName.Should().Be("Test name");
        }

        [Fact]
        public async Task DeleteLog_ValidState_ReturnNoContent()
        {
            // arrange
            var url = "api/logs";
            var postSeverityResponse = await Connection.AppClient.PostAsJsonAsync("api/severities",
                new CreationSeverityDto()
                {
                    SeverityName = "Test name",
                    SeverityDescription = "Test description"
                });
            var createdSeverityId = await postSeverityResponse.Content.ReadFromJsonAsync<int>();

            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<Guid>();

            // act
            var deleteResponse = await Connection.AppClient.DeleteAsync($"{url}/{createdId}");

            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            await Connection.AppClient.DeleteAsync($"api/logs/purge/{createdId}");
            await Connection.AppClient.DeleteAsync($"api/severities/purge/{createdSeverityId}");

            // assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<ErrorDetails>();
            createdModel.Message.Should().Be("Log not found.");
            createdModel.Details.Should().Be("Cannot find log with given id.");
            createdModel.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateLog_ValidState_ReturnNoContent()
        {
            // arrange
            var url = "api/logs";
            var postSeverityResponse = await Connection.AppClient.PostAsJsonAsync("api/severities",
                new CreationSeverityDto()
                {
                    SeverityName = "Test name",
                    SeverityDescription = "Test description"
                });
            var createdSeverityId = await postSeverityResponse.Content.ReadFromJsonAsync<int>();

            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<Guid>();

            // act
            CreationModel = Fixture.Build<CreationLogDto>()
                .With(x => x.Message, "Update message")
                .With(x => x.Severity, "Test name")
                .Create();

            var updateResponse = await Connection.AppClient.PutAsJsonAsync($"{url}/{createdId}", CreationModel);

            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<LogDto>();

            await Connection.AppClient.DeleteAsync($"api/logs/purge/{createdId}");
            await Connection.AppClient.DeleteAsync($"api/severities/purge/{createdSeverityId}");

            // assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<LogDto>();
            createdModel.Message.Should().Be("Update message");
            createdModel.SeverityName.Should().Be("Test name");
        }
    } 
}
