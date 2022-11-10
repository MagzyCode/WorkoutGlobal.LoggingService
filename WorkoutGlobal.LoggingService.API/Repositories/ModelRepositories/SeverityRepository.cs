using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.API.DbContext;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.Api.Repositories
{
    /// <summary>
    /// Represents repository for severity model.
    /// </summary>
    public class SeverityRepository : BaseRepository<Severity, int>, ISeverityRepository
    {
        /// <summary>
        /// Ctor for severity repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="context">Database context.</param>
        public SeverityRepository(
            IConfiguration configuration, 
            LoggerContext context) 
            : base(configuration, context)
        { }

        /// <summary>
        /// Create severity.
        /// </summary>
        /// <param name="creationSeverity">Creation severity.</param>
        /// <returns>Return generated id for new severity.</returns>
        public async Task<int> CreateSeverityAsync(Severity creationSeverity)
        {
            var createdId = await CreateAsync(creationSeverity);

            await SaveChangesAsync();

            return createdId;
        }

        /// <summary>
        /// Delete severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns></returns>
        public async Task DeleteSeverityAsync(int id)
        {
            await DeleteAsync(id);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get all severities.
        /// </summary>
        /// <returns>Returns collection of all severities.</returns>
        public async Task<IEnumerable<Severity>> GetAllSeveritiesAsync()
        {
            var models = await GetAll().ToListAsync();

            return models;
        }

        /// <summary>
        /// Get all logs of sevetiry level.
        /// </summary>
        /// <param name="id">Severity level id.</param>
        /// <returns>Returns collection of severity level logs.</returns>
        public async Task<IEnumerable<Log>> GetAllSeverityLogs(int id)
        {
            var logs = await Context.Logs.Where(log => log.SeverityId == id).ToListAsync();
            
            return logs;
        }

        /// <summary>
        /// Get severity by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <param name="trackChanges">Tracking model state.</param>
        /// <returns>Returns severity by given id.</returns>
        public async Task<Severity> GetSeverityAsync(int id, bool trackChanges = true)
        {
            var model = await GetAsync(id, trackChanges);

            return model;
        }

        /// <summary>
        /// Get sevetiry id by name.
        /// </summary>
        /// <param name="name">Severity name.</param>
        /// <returns>Returns severity id by given name.</returns>
        public async Task<int> GetSeverityIdByName(string name)
        {
            var id = await Context.Severities
                .Where(severity => severity.SeverityName == name)
                .Select(severity => severity.Id)
                .FirstOrDefaultAsync();

            return id;
        }

        /// <summary>
        /// Get severity name by id.
        /// </summary>
        /// <param name="id">Severity id.</param>
        /// <returns>Returns severity name by given id.</returns>
        public async Task<string> GetSeverityNameById(int id)
        {
            var name = await Context.Severities
                .Where(severity => severity.Id == id)
                .Select(severity => severity.SeverityName)
                .FirstOrDefaultAsync();

            return name;
        }

        /// <summary>
        /// Update severity.
        /// </summary>
        /// <param name="updationSeverity">Updation severity.</param>
        /// <returns></returns>
        public async Task UpdateSeverityAsync(Severity updationSeverity)
        {
            Update(updationSeverity);
            await SaveChangesAsync();
        }
    }
}
