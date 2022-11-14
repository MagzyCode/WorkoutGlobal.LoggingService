using FluentAssertions;
using WorkoutGlobal.LoggingService.Api.Repositories;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.UnitTests.Repositories
{
    public class LogRepositoryTests
    {
        public LogRepository LogRepository { get; set; } = new(null);

        [Fact]
        public async Task CreateLogAsync_NullParam_ReturnArgumentNullException()
        {
            // arrange
            Log log = null;

            // act
            var result = async () => await LogRepository.CreateLogAsync(log);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteLogAsync_EmptyParam_ArgumentNullException()
        {
            // arrange
            var guid = Guid.Empty;

            // act
            var result = async () => await LogRepository.DeleteLogAsync(guid);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetLogAsync_EmptyParam_ArgumentNullException()
        {
            // arrange
            var guid = Guid.Empty;

            // act
            var result = async () => await LogRepository.GetLogAsync(guid);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetLogSevetiry_EmptyParam_ArgumentNullException()
        {
            // arrange
            var guid = Guid.Empty;

            // act
            var result = async () => await LogRepository.GetLogSevetiry(guid);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateLogAsync_NullParam_ReturnArgumentNullException()
        {
            // arrange
            Log log = null;

            // act
            var result = async () => await LogRepository.UpdateLogAsync(log);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
