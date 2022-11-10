using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.LoggingService.API.DbContext;

namespace WorkoutGlobal.LoggingService.API.Extensions
{
    /// <summary>
    /// Base class for all service extensions.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure database settings.
        /// </summary>
        /// <param name="services">Project services.</param>
        /// <param name="configuration">Project configuration.</param>
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<LoggerContext>(
                opts => opts.UseNpgsql(
                    connectionString: configuration.GetConnectionString("LocalPostgre"),
                    npgsqlOptionsAction: b => b.MigrationsAssembly("WorkoutGlobal.LoggingService.Api")));


    }
}
