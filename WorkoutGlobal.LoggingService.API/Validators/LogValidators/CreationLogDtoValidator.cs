using FluentValidation;
using WorkoutGlobal.LoggingService.Api.Dto;

namespace WorkoutGlobal.LoggingService.Api.Validators
{
    /// <summary>
    /// Validator for CreationLogDto model.
    /// </summary>
    public class CreationLogDtoValidator : AbstractValidator<CreationLogDto>
    {
        /// <summary>
        /// Ctor for validator.
        /// </summary>
        public CreationLogDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(log => log.Message)
                .NotEmpty();

            RuleFor(log => log.Severity)
                .NotEmpty();
        }
    }
}
