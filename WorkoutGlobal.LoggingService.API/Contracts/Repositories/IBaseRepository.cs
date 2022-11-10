using WorkoutGlobal.LoggingService.API.DbContext;

namespace WorkoutGlobal.LoggingService.Api.Contracts
{
    /// <summary>
    /// Base interface for all repositories.
    /// </summary>
    public interface IBaseRepository<TModel, TId>
    {
        /// <summary>
        /// Databse context.
        /// </summary>
        public LoggerContext Context { get; }

        /// <summary>
        /// Project configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Get model by id.
        /// </summary>
        /// <param name="id">Model id.</param>
        /// <param name="trackChanges">Track changes state.</param>
        /// <returns>Returns model by given id.</returns>
        public Task<TModel> GetAsync(TId id, bool trackChanges);

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>Returns all models.</returns>
        public IQueryable<TModel> GetAll();

        /// <summary>
        /// Create model.
        /// </summary>
        /// <param name="creationModel">Creation model.</param>
        /// <returns>Returns created id.</returns>
        public Task<TId> CreateAsync(TModel creationModel);

        /// <summary>
        /// Delete model by id.
        /// </summary>
        /// <param name="id">Model id.</param>
        /// <returns></returns>
        public Task DeleteAsync(TId id);

        /// <summary>
        /// Update model.
        /// </summary>
        /// <param name="updationModel">Updation model.</param>
        public void Update(TModel updationModel);

        /// <summary>
        /// Asynchronous save changes in database.
        /// </summary>
        /// <returns>A task that represents asynchronous SaveChanges operaion.</returns>
        public Task SaveChangesAsync();
    }
}
