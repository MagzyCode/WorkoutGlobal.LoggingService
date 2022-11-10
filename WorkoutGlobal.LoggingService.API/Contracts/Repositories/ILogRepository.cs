using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Contracts
{
    /// <summary>
    /// Base interface for log repository.
    /// </summary>
    public interface ILogRepository
    {
        /// <summary>
        /// Get log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <param name="trackChanges">Tracking model state.</param>
        /// <returns>Returns log by given id.</returns>
        public Task<Log> GetLogAsync(Guid id, bool trackChanges = true);

        /// <summary>
        /// Get all logs.
        /// </summary>
        /// <returns>Returns collection of all logs.</returns>
        public Task<IEnumerable<Log>> GetAllLogsAsync();

        /// <summary>
        /// Create log.
        /// </summary>
        /// <param name="creationLog">Creation log.</param>
        /// <returns>Return generated id for new model.</returns>
        public Task<Guid> CreateLogAsync(Log creationLog);

        /// <summary>
        /// Delete log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns></returns>
        public Task DeleteLogAsync(Guid id);

        /// <summary>
        /// Update log.
        /// </summary>
        /// <param name="updationLog">Updation log.</param>
        /// <returns></returns>
        public Task UpdateLogAsync(Log updationLog);

        /// <summary>
        /// Get log severity level.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns>Severity model of giving by id log.</returns>
        public Task<Severity> GetLogSevetiry(Guid id);
    }
}
