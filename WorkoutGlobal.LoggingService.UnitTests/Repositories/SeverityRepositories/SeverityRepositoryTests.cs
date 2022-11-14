using FluentAssertions;
using WorkoutGlobal.LoggingService.Api.Repositories;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.UnitTests.Repositories
{
    public class SeverityRepositoryTests
    {
        public SeverityRepository SeverityRepository { get; set; } = new(null);

        [Fact]
        public async Task CreateSeverityAsync_NullParam_ArgumentNullException()
        {
            // arrange
            Severity severity = null;

            // act
            var result = async () => await SeverityRepository.CreateSeverityAsync(severity);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteSeverityAsync_ZeroParam_ArgumentException()
        {
            // arrange
            var id = 0;

            // act
            var result = async () => await SeverityRepository.DeleteSeverityAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetAllSeverityLogsAsync_ZeroParam_ArgumentException()
        {
            // arrange
            var id = 0;

            // act
            var result = async () => await SeverityRepository.GetAllSeverityLogsAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetSeverityAsync_ZeroParam_ArgumentException()
        {
            // arrange
            var id = 0;

            // act
            var result = async () => await SeverityRepository.GetSeverityAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetSeverityIdByName_EmptyParam_ArgumentException()
        {
            // arrange
            var name = string.Empty;

            // act
            var result = async () => await SeverityRepository.GetSeverityIdByName(name);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetSeverityNameById_ZeroParam_ArgumentException()
        {
            // arrange
            var id = 0;

            // act
            var result = async () => await SeverityRepository.GetSeverityNameById(id);

            // assert
            await result.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateSeverityAsync_NullParam_ArgumentNullException()
        {
            // arrange
            Severity severity = null;

            // act
            var result = async () => await SeverityRepository.UpdateSeverityAsync(severity);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
