using AutoFixture;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Net;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.API.Models;
using FluentAssertions;
using System;

namespace WorkoutGlobal.LoggingService.IntegrationTests.Controllers
{
    public class SeverityControllerTests : IAsyncLifetime
    {
        public AppTestConnection<int> Connection { get; set; } = new();

        public Fixture Fixture { get; set; } = new();

        public CreationSeverityDto CreationModel { get; set; }

        public async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                Connection.PurgeList.Clear();

                CreationModel = Fixture.Build<CreationSeverityDto>()
                    .With(x => x.SeverityName, "Test name")
                    .With(x => x.SeverityDescription, "Test description")
                    .Create();
            });
        }

        public async Task DisposeAsync()
        {
            foreach (var id in Connection.PurgeList)
                await Connection.AppClient.DeleteAsync($"api/severities/purge/{id}");
        }

        [Fact]
        public async Task GetSeverity_ValidState_ReturnOk()
        {
            // arrange
            var url = "api/severities";
            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<int>();

            Connection.PurgeList.Add(createdId);

            // act
            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<SeverityDto>();

            // assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            createdId.Should().BeOfType(typeof(int));
            createdId.Should().BeGreaterThanOrEqualTo(1);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<SeverityDto>();
            createdModel.SeverityName.Should().Be("Test name");
            createdModel.SeverityDescription.Should().Be("Test description");
        }

        [Fact]
        public async Task GetAllSeverities_ValidState_ReturnOk()
        {
            // arrange
            var url = "api/severities";
            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<int>();

            Connection.PurgeList.Add(createdId);

            // act
            var getResponse = await Connection.AppClient.GetAsync(url);
            var createdModel = await getResponse.Content.ReadFromJsonAsync<List<SeverityDto>>();

            // assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            createdId.Should().BeOfType(typeof(int));
            createdId.Should().BeGreaterThanOrEqualTo(1);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<List<SeverityDto>>();
            createdModel.Count.Should().BeGreaterThanOrEqualTo(1);
        }


        [Fact]
        public async Task CreateSeverity_NullParam_ReturnBadRequest()
        {
            // arrange
            CreationModel = null;

            // act
            var postResponse = await Connection.AppClient.PostAsJsonAsync("api/severities", CreationModel);
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
        public async Task CreateSeverity_InvalidState_ReturnBadRequest()
        {
            // arrange
            CreationModel = Fixture.Build<CreationSeverityDto>()
                .With(x => x.SeverityName, string.Empty)
                .With(x => x.SeverityDescription, string.Empty)
                .Create();

            // act
            var postResponse = await Connection.AppClient.PostAsJsonAsync("api/severities", CreationModel);
            var error = await postResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            error.Should().NotBeNull();
            error.Should().BeOfType<ErrorDetails>();
            error.Message.Should().Be("Incoming creation DTO model isn't valid.");
            error.Details.Should().Be("'Severity Name' должно быть заполнено.\r\n'Severity Description' должно быть заполнено.");
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateSeverity_ValidState_ReturnCreated()
        {
            // arrange
            var url = "api/severities";

            // act
            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<int>();

            Connection.PurgeList.Add(createdId);
            
            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<SeverityDto>();

            // assert
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            createdId.Should().BeOfType(typeof(int));
            createdId.Should().BeGreaterThanOrEqualTo(1);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<SeverityDto>();
            createdModel.SeverityName.Should().Be("Test name");
            createdModel.SeverityDescription.Should().Be("Test description");
        }

        [Fact]
        public async Task DeleteSeverity_ValidState_ReturnNoContent()
        {
            // arrange
            var url = "api/severities";
            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<int>();

            Connection.PurgeList.Add(createdId);

            // act
            var deleteResponse = await Connection.AppClient.DeleteAsync($"{url}/{createdId}");

            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<ErrorDetails>();
            createdModel.Message.Should().Be("Severity not found.");
            createdModel.Details.Should().Be("Cannot find severity with given id.");
            createdModel.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateSeverity_ValidState_ReturnNoContent()
        {
            // arrange
            var url = "api/severities";
            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<int>();

            Connection.PurgeList.Add(createdId);

            // act
            CreationModel = Fixture.Build<CreationSeverityDto>()
                .With(x => x.SeverityName, "Update severity name")
                .With(x => x.SeverityDescription, "Update severity description")
                .Create();

            var updateResponse = await Connection.AppClient.PutAsJsonAsync($"{url}/{createdId}", CreationModel);

            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<SeverityDto>();

            // assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<SeverityDto>();
            createdModel.SeverityName.Should().Be("Update severity name");
            createdModel.SeverityDescription.Should().Be("Update severity description");
        }

        [Fact]
        public async Task GetSeverityLogs_ValidState_ReturnOk()
        {
            // arrange
            var url = "api/severities";
            var postResponse = await Connection.AppClient.PostAsJsonAsync(url, CreationModel);
            var createdId = await postResponse.Content.ReadFromJsonAsync<int>();

            Connection.PurgeList.Add(createdId);

            var creationLog = Fixture.Build<CreationLogDto>()
                .With(x => x.Message, "Test log")
                .With(x => x.Severity, "Test name")
                .Create();
            var postLogResponse = await Connection.AppClient.PostAsJsonAsync("api/logs", creationLog);
            var createdLogId = await postLogResponse.Content.ReadFromJsonAsync<Guid>();

            // act
            var getResponse = await Connection.AppClient.GetAsync($"{url}/{createdId}/logs");
            var createdModel = await getResponse.Content.ReadFromJsonAsync<List<LogDto>>();

            await Connection.AppClient.DeleteAsync($"api/severities/purge/{createdId}");
            await Connection.AppClient.DeleteAsync($"api/logs/purge/{createdLogId}");

            // assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdModel.Should().NotBeNull();
            createdModel.Should().BeOfType<List<LogDto>>();
            createdModel.Count.Should().BeGreaterThanOrEqualTo(1);
        }
    }
}
