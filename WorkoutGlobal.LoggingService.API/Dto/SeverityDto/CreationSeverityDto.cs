namespace WorkoutGlobal.LoggingService.Api.Dto
{
    /// <summary>
    /// Represents DTO model of severity for POST method.
    /// </summary>
    public class CreationSeverityDto
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
