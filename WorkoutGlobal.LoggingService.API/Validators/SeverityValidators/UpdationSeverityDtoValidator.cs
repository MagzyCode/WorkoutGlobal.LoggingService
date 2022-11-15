using FluentValidation;
using WorkoutGlobal.LoggingService.Api.Dto;

namespace WorkoutGlobal.LoggingService.Api.Validators
{
    /// <summary>
    /// Validator for UpdationSeverityDto model.
    /// </summary>
    public class UpdationSeverityDtoValidator : AbstractValidator<UpdationSeverityDto>
    {
        /// <summary>
        /// Ctor for validator.
        /// </summary>
        public UpdationSeverityDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(severity => severity.SeverityName)
                .NotEmpty();

            RuleFor(severity => severity.SeverityDescription)
                .NotEmpty();
        }
    }
}
