using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Enums.Test;
using MoravianStar_Demo.Maintenance.Core.DTOs;
using MoravianStar_Demo.Maintenance.Core.Enums;
using MoravianStar_Demo.Persistence.DbContexts;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Maintenance.Services.Services
{
    public class DbUpdater : IDbUpdater
    {
        private readonly IConfiguration configuration;
        private readonly SystemContext systemDbContext;
        private readonly IDbContextFactory<ClientContext> clientDbContextFactory;
        private readonly IDbContextFactory<ClientDMLContext> clientDMLDbContextFactory;

        public DbUpdater(
            IConfiguration configuration,
            SystemContext systemDbContext,
            IDbContextFactory<ClientContext> clientDbContextFactory,
            IDbContextFactory<ClientDMLContext> clientDMLDbContextFactory)
        {
            this.configuration = configuration;
            this.systemDbContext = systemDbContext;
            this.clientDbContextFactory = clientDbContextFactory;
            this.clientDMLDbContextFactory = clientDMLDbContextFactory;
        }

        public async Task<DbsUpdateResult> CreateAndUpdateAllAsync()
        {
            var dbsUpdateResult = new DbsUpdateResult();
            dbsUpdateResult.Results.AddRange(new[]
            {
                new DbUpdateResult() { Name = "System" },
                new DbUpdateResult() { Name = "Empty" }
            });

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

        private async Task MigrateAndSeedSystemDB()
        {
            var systemDbCreator = (IRelationalDatabaseCreator)systemDbContext.GetInfrastructure().GetRequiredService<IDatabaseCreator>();
            if (!await systemDbCreator.ExistsAsync())
            {
                await systemDbCreator.CreateAsync();
            }

            using (var tx = await systemDbContext.Database.BeginTransactionAsync())
            {
                await systemDbContext.Database.MigrateAsync();
                await SeedSystemDbAsync(systemDbContext);
                //throw new Exception("Test");
                await tx.CommitAsync();
            }
        }

        private async Task MigrateAndSeedEmptyDB()
        {
            var emptyDbConnectionString = configuration["ConnectionStrings:Empty"];

            using (var emptyDbContext = await clientDbContextFactory.CreateDbContextAsync())
            {
                emptyDbContext.Database.SetConnectionString(emptyDbConnectionString);

                var emptyDbCreator = (IRelationalDatabaseCreator)emptyDbContext.GetInfrastructure().GetRequiredService<IDatabaseCreator>();
                if (!await emptyDbCreator.ExistsAsync())
                {
                    await emptyDbCreator.CreateAsync();
                }

                using (var emptyDMLDbContext = await clientDMLDbContextFactory.CreateDbContextAsync())
                {
                    emptyDMLDbContext.Database.SetDbConnection(emptyDbContext.Database.GetDbConnection());

                    using (var tx = await emptyDbContext.Database.BeginTransactionAsync())
                    {
                        await emptyDbContext.Database.MigrateAsync();

                        await emptyDMLDbContext.Database.UseTransactionAsync(tx.GetDbTransaction());
                        await SeedEmptyDbAsync(emptyDMLDbContext);

                        //throw new Exception("Test");

                        await tx.CommitAsync();
                    }
                }
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

                var clientDbCreator = (IRelationalDatabaseCreator)clientDbContext.GetInfrastructure().GetRequiredService<IDatabaseCreator>();
                if (!await clientDbCreator.ExistsAsync())
                {
                    await clientDbCreator.CreateAsync();
                }

                using (var clientDMLDbContext = await clientDMLDbContextFactory.CreateDbContextAsync())
                {
                    clientDMLDbContext.Database.SetDbConnection(clientDbContext.Database.GetDbConnection());

                    using (var tx = await clientDbContext.Database.BeginTransactionAsync())
                    {
                        await clientDbContext.Database.MigrateAsync();

                        await clientDMLDbContext.Database.UseTransactionAsync(tx.GetDbTransaction());
                        await SeedClientDbAsync(clientDMLDbContext, clientId);

                        //throw new Exception("Test");

                        await tx.CommitAsync();
                    }
                }
            }
        }

        private async Task SeedSystemDbAsync(SystemContext systemDbContext)
        {
            var languageExist = await systemDbContext.Set<LanguageEntity>().AnyAsync();
            if (!languageExist)
            {
                var languages = new List<LanguageEntity>()
                {
                    new LanguageEntity() { Name = "English" },
                    new LanguageEntity() { Name = "Bulgarian" },
                    new LanguageEntity() { Name = "Romanian" },
                    new LanguageEntity() { Name = "Ukrainian" },
                    new LanguageEntity() { Name = "French" },
                    new LanguageEntity() { Name = "German" },
                    new LanguageEntity() { Name = "Italian" },
                    new LanguageEntity() { Name = "Spanish" },
                    new LanguageEntity() { Name = "Russian" },
                    new LanguageEntity() { Name = "Chinese" },
                    new LanguageEntity() { Name = "Japanese" },
                    new LanguageEntity() { Name = "Korean" },
                };

                foreach (var language in languages)
                {
                    await systemDbContext.Set<LanguageEntity>().AddAsync(language);
                }
            }

            // Create "Client A" if not exist
            var clientAExist = await systemDbContext.Set<ClientEntity>().Where(x => x.Name == "Client A").AnyAsync();
            ClientEntity clientA = null;
            if (!clientAExist)
            {
                clientA = (await systemDbContext.Set<ClientEntity>().AddAsync(new ClientEntity()
                {
                    Name = "Client A",
                    Description = "Client A is our precious client",
                    Status = ClientStatus.Active,
                    MainAddress = new AddressEntity()
                    {
                        Address = "Sofia"
                    },
                    Addresses = new List<AddressEntity>()
                    {
                        new AddressEntity() { Address = "Plovdiv" },
                        new AddressEntity() { Address = "Varna" }
                    }
                })).Entity;
            }

            // Create "Vehicle B1111AA" if not exist
            var vehicleB1111AAExist = await systemDbContext.Set<VehicleEntity>().Where(x => x.LicensePlate == "B1111AA").AnyAsync();
            if (!vehicleB1111AAExist)
            {
                await systemDbContext.Set<VehicleEntity>().AddAsync(new VehicleEntity()
                {
                    LicensePlate = "B1111AA",
                    CurrentLocation = new Point(43.21417685342469, 27.92603583611857) { SRID = 4326 },
                    Clients = new List<ClientEntity>() { clientA }
                });
            }

            // Save changes
            await systemDbContext.SaveChangesAsync();
        }

        private async Task SeedEmptyDbAsync(ClientDMLContext emptyDMLDbContext)
        {
            // Create example: Create a new client in the system DB using empty (client) DbContext
            var client = new ClientEntity()
            {
                Name = "Client " + DateTime.Now.Ticks
            };
            client = (await emptyDMLDbContext.Set<ClientEntity>().AddAsync(client)).Entity;

            // Save changes
            await emptyDMLDbContext.SaveChangesAsync();

            // Insert many-to-many example: Create a new ClientEntity (see previous step) and attach it to the created BlockEntity
            var gf = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            await emptyDMLDbContext.Set<BlockEntity>().AddAsync(new BlockEntity()
            {
                Boundaries = gf.CreatePolygon(new Coordinate[4]
                {
                    new Coordinate(43.31562369668484, 24.72116775363997),
                    new Coordinate(42.74555811024697, 25.85196988681144),
                    new Coordinate(42.44255654569372, 24.42655263848448),
                    new Coordinate(43.31562369668484, 24.72116775363997)
                }),
                Client = client
            });

            // Save changes
            await emptyDMLDbContext.SaveChangesAsync();
        }

        private async Task SeedClientDbAsync(ClientDMLContext emptyDMLDbContext, int clientId)
        {
            await SeedEmptyDbAsync(emptyDMLDbContext);
        }
    }
}