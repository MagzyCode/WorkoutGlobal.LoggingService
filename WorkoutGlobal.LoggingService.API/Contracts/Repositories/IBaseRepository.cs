using Npgsql;

namespace WorkoutGlobal.LoggingService.Api.Contracts
{
    /// <summary>
    /// Base interface for all repositories.
    /// </summary>
    public interface IBaseRepository
    {
        /// <summary>
        /// Project configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Connection string to database.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Opens connection to Postgre database.
        /// </summary>
        /// <returns>Returns database connection.</returns>
        public NpgsqlConnection OpenConnection();
    }
}
