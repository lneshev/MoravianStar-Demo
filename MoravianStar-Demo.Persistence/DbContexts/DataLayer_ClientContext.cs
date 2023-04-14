using Microsoft.EntityFrameworkCore;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Persistence.Attributes;
using MoravianStar_Demo.Persistence.Constants;
using MoravianStar_Demo.Persistence.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace MoravianStar_Demo.Persistence.DbContexts
{
    /// <summary>
    /// A DbContext for working with "Empty" and "Client" databases.<br/>
    /// <b>Put here all db objects as usual, except for SQL synonyms. In this context they should be defined as views, so that no code is generated in the migrations for them!<br/>
    /// Use this DbContext for DDL operations and creating migrations.</b>
    /// </summary>
    public class DataLayer_ClientContext : DbContext
    {
        public DataLayer_ClientContext()
        {
        }

        public DataLayer_ClientContext(DbContextOptions<DataLayer_ClientContext> options) : base(options)
        {
        }

        protected DataLayer_ClientContext(DbContextOptions options) : base(options)
        {
        }

        //public virtual DbSet<BlockEntity> Blocks { get; set; }
        //public virtual DbSet<ClientEntity> Clients { get; set; }

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

            #region Synonyms. Add the synonyms here as views, so that the migration do not generate tables.
            #region Many-to-many synonyms
            modelBuilder.ManyToMany<ClientEntity, VehicleEntity>(x => x.Vehicles, x => x.Clients, "ClientId", "VehicleId", DbSchemaConstants.ClientVehicle).ToView(DbSchemaConstants.ClientVehicle);
            #endregion

            #region Table synonyms
            modelBuilder.Entity<ClientEntity>().ToView(DbSchemaConstants.Client);
            modelBuilder.Entity<AddressEntity>().ToView(DbSchemaConstants.Address);
            modelBuilder.Entity<VehicleEntity>().ToView(DbSchemaConstants.Vehicle);
            #endregion
            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(
                GetType().Assembly,
                x => x.GetCustomAttributes<ForDbContextAttribute>().FirstOrDefault(y => y.DbContextType == typeof(DataLayer_ClientContext)) != null);
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

    /// <summary>
    /// A DbContext for working with "Empty" and "Client" databases.<br/>
    /// <b>Here do not put any db objects and logics, except for SQL synonyms. In this context they should be defined as tables, so that they can be corectly recognized as entities!<br/>
    /// Use this DbContext for DML operations.</b>
    /// </summary>
    public class DataLayer_ClientDMLContext : DataLayer_ClientContext
    {
        public DataLayer_ClientDMLContext(DbContextOptions<DataLayer_ClientDMLContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add the synonyms here as tables, so that the context work with them like tables
            modelBuilder.ApplyConfigurationsFromAssembly(
                GetType().Assembly,
                x => x.GetCustomAttributes<ForDbContextAttribute>().FirstOrDefault(y => y.DbContextType == typeof(DataLayer_SystemContext) && y.IsSynonymInTheOtherContext) != null);

            modelBuilder.ManyToMany<ClientEntity, VehicleEntity>(x => x.Vehicles, x => x.Clients, "ClientId", "VehicleId", DbSchemaConstants.ClientVehicle);
        }
    }
}