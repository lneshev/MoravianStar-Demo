using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MoravianStar_Demo.Common.Jobs.Client
{
    /// <summary>
    /// A DbContext for working with "Hangfire" database.
    /// </summary>
    public class HangfireContext : DbContext
    {
        public HangfireContext(DbContextOptions options) : base(options)
        {
        }
    }
}