using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.API.DbContext;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Repositories.ModelRepositories
{
    /// <summary>
    /// Represents repository for log model.
    /// </summary>
    public class LogRepository : BaseRepository<Log, Guid>, ILogRepository
    {
        /// <summary>
        /// Ctor for log repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="context">Database context.</param>
        public LogRepository(
            IConfiguration configuration, 
            LoggerContext context) 
            : base(configuration, context)
        { }

        /// <summary>
        /// Create log.
        /// </summary>
        /// <param name="creationLog">Creation log.</param>
        /// <returns>Return generated id for new model.</returns>
        public async Task<Guid> CreateLogAsync(Log creationLog)
        {
            var createdId = await CreateAsync(creationLog);

            await SaveChangesAsync();

            return createdId;
        }

        /// <summary>
        /// Delete log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns></returns>
        public async Task DeleteLogAsync(Guid id)
        {
            await DeleteAsync(id);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get all logs.
        /// </summary>
        /// <returns>Returns collection of all logs.</returns>
        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            var models = await GetAll().ToListAsync();

            return models;
        }

        /// <summary>
        /// Get log by id.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <param name="trackChanges">Tracking model change.</param>
        /// <returns>Returns log by given id.</returns>
        public async Task<Log> GetLogAsync(Guid id, bool trackChanges = true)
        {
            var model = await GetAsync(id, trackChanges);

            return model;
        }

        /// <summary>
        /// Get severity level of log.
        /// </summary>
        /// <param name="id">Log id.</param>
        /// <returns>Severity level of log.</returns>
        public async Task<Severity> GetLogSevetiry(Guid id)
        {
            var log = await GetAsync(id);

            var severity = await Context.Severities.Where(severity => severity.Id == log.SeverityId).FirstOrDefaultAsync();

            return severity;
        }

        /// <summary>
        /// Update log.
        /// </summary>
        /// <param name="updationLog">Updation log.</param>
        /// <returns></returns>
        public async Task UpdateLogAsync(Log updationLog)
        {
            Update(updationLog);
            await SaveChangesAsync();
        }
    }
}
