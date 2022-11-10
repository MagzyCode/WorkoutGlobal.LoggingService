using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WorkoutGlobal.LoggingService.Api.Contracts;
using WorkoutGlobal.LoggingService.API.DbContext;

namespace WorkoutGlobal.LoggingService.Api.Repositories
{
    /// <summary>
    /// Represents base repository for all model repositories.
    /// </summary>
    public class BaseRepository<TModel, TId> : IBaseRepository<TModel, TId>
        where TModel : class, IModel<TId>
    {
        private LoggerContext _context;
        private IConfiguration _configuration;

        /// <summary>
        /// Ctor for base repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="context">Database context.</param>
        public BaseRepository(
            IConfiguration configuration,
            LoggerContext context)
        {
            Configuration = configuration;
            Context = context;
        }

        /// <summary>
        /// Project configuration.
        /// </summary>
        public IConfiguration Configuration
        {
            get => _configuration;
            private set => _configuration = value;
        }

        /// <summary>
        /// Databse context.
        /// </summary>
        public LoggerContext Context
        {
            get => _context;
            private set => _context = value;
        }

        /// <summary>
        /// Create model.
        /// </summary>
        /// <param name="creationModel">Creation model.</param>
        /// <returns>Returns created id.</returns>
        public async Task<TId> CreateAsync(TModel creationModel)
        {
            if (creationModel is null)
                throw new ArgumentNullException(nameof(creationModel), "Creation model cannot be null.");

            await Context.Set<TModel>().AddAsync(creationModel);

            return creationModel.Id;
        }

        /// <summary>
        /// Delete model by id.
        /// </summary>
        /// <param name="id">Model id.</param>
        /// <returns></returns>
        public async Task DeleteAsync(TId id)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default))
                throw new ArgumentNullException(nameof(id), "Searchable id cannot have default value.");

            var deletionModel = await GetAsync(id);

            Context.Set<TModel>().Remove(deletionModel);
        }

        /// <summary>
        /// Get all models.
        /// </summary>
        /// <returns>Returns all models.</returns>
        public IQueryable<TModel> GetAll()
        {
            var models = Context.Set<TModel>();

            return models;
        }

        /// <summary>
        /// Get model by id.
        /// </summary>
        /// <param name="id">Model id.</param>
        /// <param name="trackChanges">Tracking model state.</param>
        /// <returns>Returns model by given id.</returns>
        public async Task<TModel> GetAsync(TId id, bool trackChanges = true)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default))
                throw new ArgumentNullException(nameof(id), "Searchable id cannot have default value.");

            var model = trackChanges
                ? await Context.Set<TModel>().FindAsync(id)
                : await Context.Set<TModel>().AsNoTracking()
                    .Where(x => x.Id.Equals(id))
                    .FirstOrDefaultAsync();

            return model;
        }

        /// <summary>
        /// Update model.
        /// </summary>
        /// <param name="updationModel">Updation model.</param>
        public void Update(TModel updationModel)
        {
            if (updationModel is null)
                throw new ArgumentNullException(nameof(updationModel), "Updation model cannot be null.");

            Context.Set<TModel>().Update(updationModel);
        }

        /// <summary>
        /// Asynchronous save changes in database.
        /// </summary>
        /// <returns>A task that represents asynchronous SaveChanges operaion.</returns>
        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
