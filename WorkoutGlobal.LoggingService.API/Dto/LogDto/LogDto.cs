namespace WorkoutGlobal.LoggingService.Api.Dto
{
    /// <summary>
    /// Represents DTO model of log for GET method.
    /// </summary>
    public class LogDto
    {
        /// <summary>
        /// Unique identifier of log.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Time when log was created.
        /// </summary>
        public DateTime LogTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Log info.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Represents severity name.
        /// </summary>
        public string SeverityName { get; set; }
    }
}
