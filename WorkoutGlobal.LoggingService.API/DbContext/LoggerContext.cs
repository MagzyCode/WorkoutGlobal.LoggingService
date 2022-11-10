using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.LoggingService.API.Models;

namespace WorkoutGlobal.LoggingService.API.DbContext
{
    /// <summary>
    /// Represents database context of logging microservice.
    /// </summary>
    public class LoggerContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Ctor for set course microsevice context options.
        /// </summary>
        /// <param name="options">Context options.</param>
        public LoggerContext(DbContextOptions options) : base(options)
        { }

        /// <summary>
        /// Create relations between models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Relations

            modelBuilder.Entity<Log>()
                .HasOne(log => log.SeverityLevel)
                .WithMany(level => level.Logs)
                .HasForeignKey(log => log.SeverityId)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion
        }

        /// <summary>
        /// Represents table of logs.
        /// </summary>
        public DbSet<Log> Logs { get; set; }

        /// <summary>
        /// Represents table of severities.
        /// </summary>
        public DbSet<Severity> Severities { get; set; }
    }
}
