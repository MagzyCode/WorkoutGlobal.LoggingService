﻿using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutGlobal.LoggingService.Api.Dto;
using WorkoutGlobal.LoggingService.Api.Validators;

namespace WorkoutGlobal.LoggingService.UnitTests.Validators.LogValidators
{
    public class UpdationLogDtoValidatorTests
    {
        public UpdationLogDtoValidator Validator { get; set; } = new();

        public Fixture Fixture { get; set; } = new();

        [Fact]
        public async Task ModelState_NullProperties_ReturnValidationResult()
        {
            // arrange
            var updationLogDto = new UpdationLogDto();

            // act
            var validationResult = await Validator.ValidateAsync(updationLogDto);

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
            var updationLogDto = Fixture.Build<UpdationLogDto>()
                .With(x => x.Message, string.Empty)
                .With(x => x.Severity, string.Empty)
                .Create();

            // act
            var validationResult = await Validator.ValidateAsync(updationLogDto);

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
            var updationLogDto = Fixture.Create<UpdationLogDto>();

            // act
            var validationResult = await Validator.ValidateAsync(updationLogDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(0);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
