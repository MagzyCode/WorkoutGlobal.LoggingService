using FluentValidation;
using WorkoutGlobal.LoggingService.Api.Dto;

namespace WorkoutGlobal.LoggingService.Api.Validators
{
    /// <summary>
    /// Validator for CreationSeverityDto model.
    /// </summary>
    public class CreationSeverityDtoValidator : AbstractValidator<CreationSeverityDto>
    {
        /// <summary>
        /// Ctor for validator.
        /// </summary>
        public CreationSeverityDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(severity => severity.SeverityName)
                .NotEmpty();

            RuleFor(severity => severity.SeveriryDescription)
                .NotEmpty();
        }
    }
}
