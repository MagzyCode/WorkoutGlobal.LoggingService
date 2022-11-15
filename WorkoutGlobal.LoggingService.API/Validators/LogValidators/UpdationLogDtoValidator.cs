using FluentValidation;
using WorkoutGlobal.LoggingService.Api.Dto;

namespace WorkoutGlobal.LoggingService.Api.Validators
{
    /// <summary>
    /// Validator for UpdationLogDto model.
    /// </summary>
    public class UpdationLogDtoValidator : AbstractValidator<UpdationLogDto>
    {
        /// <summary>
        /// Ctor for validator.
        /// </summary>
        public UpdationLogDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(log => log.Message)
                .NotEmpty();

            RuleFor(log => log.Severity)
                .NotEmpty();
        }
    }
}
