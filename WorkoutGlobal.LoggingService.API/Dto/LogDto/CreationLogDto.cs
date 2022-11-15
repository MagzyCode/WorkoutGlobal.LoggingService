namespace WorkoutGlobal.LoggingService.Api.Dto
{
    /// <summary>
    /// Represents DTO model of log for POST method.
    /// </summary>
    public class CreationLogDto
    {
        /// <summary>
        /// Log info.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Represents severity id for create relations.
        /// </summary>
        public string Severity { get; set; }
    }
}
