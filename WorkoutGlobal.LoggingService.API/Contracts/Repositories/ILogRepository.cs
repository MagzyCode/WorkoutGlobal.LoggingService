using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Contracts
{
    /// <summary>
    /// Base interface for log repository.
    /// </summary>
    /// <typeparam name="TId">Model identifier.</typeparam>
    /// <typeparam name="TModel">Repository operation model.</typeparam>
    public interface ILogRepository<TId, TModel>
        where TModel: IModel<TId>
    {
        /// <summary>
        /// Get log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns>Returns log by given id.</returns>
        public Task<TModel> GetLogAsync(TId id);

        /// <summary>
        /// Get all logs.
        /// </summary>
        /// <returns>Returns collection of all logs.</returns>
        public Task<IEnumerable<TModel>> GetAllLogsAsync();

        /// <summary>
        /// Create log.
        /// </summary>
        /// <param name="creationLog">Creation log.</param>
        /// <returns>Return generated id for new model.</returns>
        public Task<TId> CreateLogAsync(TModel creationLog);

        /// <summary>
        /// Delete log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns></returns>
        public Task DeleteLogAsync(TId id);

        /// <summary>
        /// Update log.
        /// </summary>
        /// <param name="updationLog">Updation log.</param>
        /// <returns></returns>
        public Task UpdateLogAsync(TModel updationLog);

        /// <summary>
        /// Get log severity level.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns>Severity model of giving by id log.</returns>
        public Task<Severity> GetLogSevetiry(TId id);
    }
}
