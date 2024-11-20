using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Maintenance.Core.DTOs;
using MoravianStar_Demo.Maintenance.Core.Enums;
using MoravianStar_Demo.Persistence.DbContexts;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Maintenance.Services.Services
{
    public class DbUpdater : IDbUpdater
    {
        private readonly IConfiguration configuration;
        private readonly LogContext logDbContext;
        private readonly SystemContext systemDbContext;
        private readonly IDbContextFactory<ClientContext> clientDbContextFactory;

        public DbUpdater(
            IConfiguration configuration,
            LogContext logDbContext,
            SystemContext systemDbContext,
            IDbContextFactory<ClientContext> clientDbContextFactory)
        {
            this.configuration = configuration;
            this.logDbContext = logDbContext;
            this.systemDbContext = systemDbContext;
            this.clientDbContextFactory = clientDbContextFactory;
        }

        public async Task<DbsUpdateResult> CreateAndUpdateAllAsync()
        {
            var dbsUpdateResult = new DbsUpdateResult();
            dbsUpdateResult.Results.AddRange(new[]
            {
                new DbUpdateResult() { Name = "Log" },
                new DbUpdateResult() { Name = "System" },
                new DbUpdateResult() { Name = "Empty" }
            });

            try
            {
                await MigrateLogDB();
                dbsUpdateResult.Results.Single(x => x.Name == "Log").State = DbUpdateState.Success;
            }
            catch (Exception ex)
            {
                DbUpdateResult dbUpdateResult = dbsUpdateResult.Results.Single(x => x.Name == "Log");
                dbUpdateResult.State = DbUpdateState.Fail;
                dbUpdateResult.Exception = ex;
                dbsUpdateResult.State = DbsUpdateState.FailNoActionNeeded;
            }

            if (dbsUpdateResult.State == DbsUpdateState.Unknown)
            {
                try
                {
                    await MigrateAndSeedSystemDB();
                    dbsUpdateResult.Results.Single(x => x.Name == "System").State = DbUpdateState.Success;
                }
                catch (Exception ex)
                {
                    DbUpdateResult dbUpdateResult = dbsUpdateResult.Results.Single(x => x.Name == "System");
                    dbUpdateResult.State = DbUpdateState.Fail;
                    dbUpdateResult.Exception = ex;
                    dbsUpdateResult.State = DbsUpdateState.FailNoActionNeeded;
                }
            }

            if (dbsUpdateResult.State == DbsUpdateState.Unknown)
            {
                try
                {
                    await MigrateAndSeedEmptyDB();
                    dbsUpdateResult.Results.Single(x => x.Name == "Empty").State = DbUpdateState.Success;
                }
                catch (Exception ex)
                {
                    DbUpdateResult dbUpdateResult = dbsUpdateResult.Results.Single(x => x.Name == "Empty");
                    dbUpdateResult.State = DbUpdateState.Fail;
                    dbUpdateResult.Exception = ex;
                    dbsUpdateResult.State = DbsUpdateState.FailActionNeeded;
                }
            }

            if (dbsUpdateResult.State == DbsUpdateState.Unknown)
            {
                await MigrateAndSeedAllClientDBs(dbsUpdateResult);
            }

            if (dbsUpdateResult.State == DbsUpdateState.Unknown)
            {
                dbsUpdateResult.State = DbsUpdateState.Success;
            }

            return dbsUpdateResult;
        }

        private async Task MigrateLogDB()
        {
            var logDbCreator = (IRelationalDatabaseCreator)logDbContext.GetInfrastructure().GetRequiredService<IDatabaseCreator>();
            if (!await logDbCreator.ExistsAsync())
            {
                await logDbCreator.CreateAsync();
            }
        }

        private async Task MigrateAndSeedSystemDB()
        {
            await systemDbContext.Database.MigrateAsync();
        }

        private async Task MigrateAndSeedEmptyDB()
        {
            var emptyDbConnectionString = configuration["ConnectionStrings:Empty"];

            using (var emptyDbContext = await clientDbContextFactory.CreateDbContextAsync())
            {
                emptyDbContext.Database.SetConnectionString(emptyDbConnectionString);
                await emptyDbContext.Database.MigrateAsync();
            }
        }

        private async Task MigrateAndSeedAllClientDBs(DbsUpdateResult dbsUpdateResult)
        {
            var clientIds = systemDbContext.Set<ClientEntity>().Select(x => x.Id).ToList();

            var concurrentDbUpdateResult = new ConcurrentBag<DbUpdateResult>();

            await Parallel.ForEachAsync(clientIds, async (clientId, cancellationToken) =>
            {
                var dbUpdateResult = new DbUpdateResult()
                {
                    Name = $"Client {clientId}"
                };
                concurrentDbUpdateResult.Add(dbUpdateResult);

                try
                {
                    await MigrateAndSeedClientDB(clientId);
                    dbUpdateResult.State = DbUpdateState.Success;
                }
                catch (Exception ex)
                {
                    dbUpdateResult.State = DbUpdateState.Fail;
                    dbUpdateResult.Exception = ex;
                }
            });

            dbsUpdateResult.Results.AddRange(concurrentDbUpdateResult.OrderBy(x => x.Name));
            if (concurrentDbUpdateResult.Any(x => x.State != DbUpdateState.Success))
            {
                dbsUpdateResult.State = DbsUpdateState.FailActionNeeded;
            }
        }

        private async Task MigrateAndSeedClientDB(int clientId)
        {
            var clientDbConnectionString = string.Format(configuration["ConnectionStrings:Client"], clientId);

            using (var clientDbContext = await clientDbContextFactory.CreateDbContextAsync())
            {
                clientDbContext.Database.SetConnectionString(clientDbConnectionString);
                await clientDbContext.Database.MigrateAsync();
            }
        }
    }
}