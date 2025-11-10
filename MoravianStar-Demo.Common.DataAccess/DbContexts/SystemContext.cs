using Microsoft.EntityFrameworkCore;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Common.DataAccess.Attributes;
using MoravianStar_Demo.Common.DataAccess.Constants;
using MoravianStar_Demo.Common.DataAccess.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace MoravianStar_Demo.Common.DataAccess.DbContexts
{
    /// <summary>
    /// A DbContext for working with "System" database.
    /// </summary>
    public class SystemContext : DbContext
    {
        public SystemContext(DbContextOptions<SystemContext> options)
            : base(options)
        {
        }

        //public DbSet<ClientEntity> Clients { get; set; }
        //public DbSet<AddressEntity> Addresses { get; set; }
        //public DbSet<VehicleEntity> Vehicles { get; set; }
        //public DbSet<LanguageEntity> Languages { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(
                GetType().Assembly,
                x => x.GetCustomAttributes<ForDbContextAttribute>().FirstOrDefault(y => y.DbContextType == typeof(SystemContext)) != null);

            modelBuilder.ManyToMany<ClientEntity, VehicleEntity>(x => x.Vehicles, x => x.Clients, "ClientId", "VehicleId", DbSchemaConstants.ClientVehicle);
        }

        private void OnBeforeSaving()
        {
            if (Database.CurrentTransaction == null)
            {
                throw new InvalidOperationException(Strings.SavingDataToDBWithoutATransactionIsNotAllowed);
            }

            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.State != EntityState.Unchanged)
                {
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        var validationContext = new ValidationContext(entry.Entity);
                        Validator.ValidateObject(entry.Entity, validationContext, true);
                    }
                }
            }
        }
    }
}