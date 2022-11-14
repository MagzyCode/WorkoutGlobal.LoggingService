using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Contracts
{
    /// <summary>
    /// Base interface for severity repository.
    /// </summary>
    /// <typeparam name="TId">Model identifier.</typeparam>
    /// <typeparam name="TModel">Repository operation model.</typeparam>
    public interface ISeverityRepository<TId, TModel>
    {
        /// <summary>
        /// Get severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns>Returns severity by given id.</returns>
        public Task<TModel> GetSeverityAsync(TId id);

        /// <summary>
        /// Get all severities.
        /// </summary>
        /// <returns>Returns collection of all severities.</returns>
        public Task<IEnumerable<TModel>> GetAllSeveritiesAsync();

        /// <summary>
        /// Create severity.
        /// </summary>
        /// <param name="creationSeverity">Creation severity.</param>
        /// <returns>Return generated id for new severity.</returns>
        public Task<TId> CreateSeverityAsync(TModel creationSeverity);

        /// <summary>
        /// Delete severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns></returns>
        public Task DeleteSeverityAsync(TId id);

        /// <summary>
        /// Update severity.
        /// </summary>
        /// <param name="updationSeverity">Updation severity.</param>
        /// <returns></returns>
        public Task UpdateSeverityAsync(TModel updationSeverity);

        /// <summary>
        /// Get all logs of sevetiry level.
        /// </summary>
        /// <param name="id">Severity level id.</param>
        /// <returns>Returns collection of severity level logs.</returns>
        public Task<IEnumerable<Log>> GetAllSeverityLogs(TId id);

        /// <summary>
        /// Get severity name by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns>Returns severity name by given id.</returns>
        public Task<string> GetSeverityNameById(TId id);

        /// <summary>
        /// Get sevetiry id by name.
        /// </summary>
        /// <param name="name">Severity name.</param>
        /// <returns>Returns severity id by given name.</returns>
        public Task<TId> GetSeverityIdByName(string name);
    }
}
