using Npgsql;
using WorkoutGlobal.LoggingService.Api.Contracts;

namespace WorkoutGlobal.LoggingService.Api.Repositories
{
    /// <summary>
    /// Represents base repository for all model repositories.
    /// </summary>
    public abstract class BaseRepository : IBaseRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        private string _tableName;

        /// <summary>
        /// Ctor for base repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>  
        /// <param name="tableName">Table name.</param>  
        public BaseRepository(IConfiguration configuration, string tableName)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("LocalPostgre");
            TableName = @$"""{tableName}""";
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
        /// Connection database string.
        /// </summary>
        public string ConnectionString
        {
            get => _connectionString;
            private set => _connectionString = value;
        }

        /// <summary>
        /// Table name.
        /// </summary>
        public string TableName
        {
            get => _tableName;
            private set => _tableName = string.IsNullOrEmpty(value)
                ? throw new NullReferenceException("Table name cannot be null or empty.")
                : value;
        }

        /// <summary>
        /// Opens connection to Postgre database.
        /// </summary>
        /// <returns>Returns database connection.</returns>
        public NpgsqlConnection OpenConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}
