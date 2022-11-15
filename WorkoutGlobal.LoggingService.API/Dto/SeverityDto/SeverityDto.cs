namespace WorkoutGlobal.LoggingService.Api.Dto
{
    /// <summary>
    /// Represents DTO model of severity for GET method.
    /// </summary>
    public class SeverityDto
    {
        /// <summary>
        /// Unique identifier of severity level.
        /// </summary>
        public int Id { get; set; }

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
