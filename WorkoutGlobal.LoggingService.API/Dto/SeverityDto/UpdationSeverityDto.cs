namespace WorkoutGlobal.LoggingService.Api.Dto
{
    /// <summary>
    /// Represents DTO model of log for PUT method.
    /// </summary>
    public class UpdationSeverityDto
    {
        /// <summary>
        /// Severity level name.
        /// </summary>
        public string SeverityName { get; set; }

        /// <summary>
        /// Severity level description.
        /// </summary>
        public string SeverityDescription { get; set; }
    }
}
