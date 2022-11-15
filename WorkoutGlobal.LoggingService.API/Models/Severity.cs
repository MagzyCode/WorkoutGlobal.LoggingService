using WorkoutGlobal.LoggingService.Api.Contracts;

namespace WorkoutGlobal.LoggingService.API.Models
{
    /// <summary>
    /// Represents model of severity level of log.
    /// </summary>
    public class Severity : IModel<int>
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

        /// <summary>
        /// Represents collection of logs with certain severity level.
        /// </summary>
        public ICollection<Log> Logs { get; set; }
    }
}
