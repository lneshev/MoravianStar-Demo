using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MoravianStar_Demo.Common.DataAccess.DbContexts
{
    /// <summary>
    /// A DbContext for working with "Log" database.
    /// </summary>
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions<LogContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");
        }
    }
}