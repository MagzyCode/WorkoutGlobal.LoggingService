using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System.Security.Cryptography;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.Api.Validators;

namespace WorkoutGlobal.LoggingService.UnitTests.Validators
{
    public class CreationLogDtoValidatorTests
    {
        public CreationLogDtoValidator Validator { get; set; } = new();

        public Fixture Fixture { get; set; } = new();

        [Fact]
        public async Task ModelState_NullProperties_ReturnValidationResult()
        {
            // arrange
            var creationLogDto = new CreationLogDto();

            // act
            var validationResult = await Validator.ValidateAsync(creationLogDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(2);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_EmptyProperties_ReturnValidationResult()
        {
            // arrange
            var creationLogDto = Fixture.Build<CreationLogDto>()
                .With(x => x.Message, string.Empty)
                .With(x => x.Severity, string.Empty)
                .Create();

            // act
            var validationResult = await Validator.ValidateAsync(creationLogDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(2);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_ValidProperties_ReturnValidationResult()
        {
            // arrange
            var creationLogDto = Fixture.Create<CreationLogDto>();

            // act
            var validationResult = await Validator.ValidateAsync(creationLogDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(0);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
