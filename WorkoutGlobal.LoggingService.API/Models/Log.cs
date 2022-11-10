namespace WorkoutGlobal.LoggingService.API.Models
{
    /// <summary>
    /// Represents basic model of log.
    /// </summary>
    public class Log
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
        /// Represents severity id for create relations.
        /// </summary>
        public int SeverityId { get; set; }

        /// <summary>
        /// Represents severity model for create relations.
        /// </summary>
        public Severity SeverityLevel { get; set; }
    }
}
