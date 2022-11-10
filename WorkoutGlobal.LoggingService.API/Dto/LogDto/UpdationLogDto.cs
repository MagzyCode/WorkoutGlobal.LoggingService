namespace WorkoutGlobal.LoggingService.Api.Dto
{
    /// <summary>
    /// Represents DTO model of log for PUT method.
    /// </summary>
    public class UpdationLogDto
    {
        /// <summary>
        /// Log info.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Represents severity name.
        /// </summary>
        public string Severity { get; set; }
    }
}
