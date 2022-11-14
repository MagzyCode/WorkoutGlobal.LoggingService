using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.Api.Validators;

namespace WorkoutGlobal.LoggingService.UnitTests.Validators
{
    public class UpdationSeverityDtoValidatorTests
    {
        public UpdationSeverityDtoValidator Validator { get; set; } = new();

        public Fixture Fixture { get; set; } = new();

        [Fact]
        public async Task ModelState_NullProperties_ReturnValidationResult()
        {
            // arrange
            var updationSeverityDto = new UpdationSeverityDto();

            // act
            var validationResult = await Validator.ValidateAsync(updationSeverityDto);

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
            var updationSeverityDto = Fixture.Build<UpdationSeverityDto>()
                .With(x => x.SeverityName, string.Empty)
                .With(x => x.SeveriryDescription, string.Empty)
                .Create();

            // act
            var validationResult = await Validator.ValidateAsync(updationSeverityDto);

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
            var updationSeverityDto = Fixture.Create<UpdationSeverityDto>();

            // act
            var validationResult = await Validator.ValidateAsync(updationSeverityDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(0);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
