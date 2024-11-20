using Microsoft.EntityFrameworkCore;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Enums.Test;
using MoravianStar_Demo.Persistence.DbContexts;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoravianStar_Demo.Maintenance.Services.Services
{
    public static class DbSeeder
    {
        public static async Task SeedSystemDbAsync(SystemContext systemDbContext)
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

        public static async Task SeedClientDbAsync(ClientDMLContext emptyDMLDbContext)
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
    }
}