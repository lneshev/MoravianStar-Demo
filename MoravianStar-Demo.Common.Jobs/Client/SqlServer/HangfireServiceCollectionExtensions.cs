using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MoravianStar_Demo.Common.Jobs.Client.SqlServer
{
    public static class HangfireServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a Hangfire functionality with SQL Server storage.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="sqlServerStorageOptions">The options for configuration of SQL storage.</param>
        /// <param name="configuration">The options for configuration of Hangfire.</param>
        /// <remarks>If the option: 'PrepareSchemaIfNecessary' is <see langword="true"/> and the database doesn't exist, it is created.</remarks>
        /// <returns>The collection of services.</returns>
        public static IServiceCollection AddHangfireWithSqlServerStorage(this IServiceCollection services, string connectionString, SqlServerStorageOptions sqlServerStorageOptions, Action<IServiceProvider, IGlobalConfiguration> configuration = null)
        {
            services.AddHangfire((sp, gc) =>
            {
                if (sqlServerStorageOptions.PrepareSchemaIfNecessary)
                {
                    using (var scope = sp.CreateAsyncScope())
                    {
                        var hangfireDbContext = scope.ServiceProvider.GetRequiredService<HangfireContext>();
                        var dbCreator = (IRelationalDatabaseCreator)hangfireDbContext.GetInfrastructure().GetRequiredService<IDatabaseCreator>();
                        if (!dbCreator.Exists())
                        {
                            dbCreator.Create();
                        }
                    }
                }

                gc.UseSqlServerStorage(connectionString, sqlServerStorageOptions);

                if (configuration != null)
                {
                    configuration(sp, gc);
                }
            });

            services
                .AddDbContext<HangfireContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            return services;
        }
    }
}