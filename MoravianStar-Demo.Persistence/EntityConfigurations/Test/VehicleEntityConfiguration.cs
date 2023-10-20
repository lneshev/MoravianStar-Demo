using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Persistence.Attributes;
using MoravianStar_Demo.Persistence.Constants;
using MoravianStar_Demo.Persistence.DbContexts;

namespace MoravianStar_Demo.Persistence.EntityConfigurations.Test
{
    [ForDbContext(typeof(SystemContext), isSynonymInTheOtherContext: true)]
    public class VehicleEntityConfiguration : IEntityTypeConfiguration<VehicleEntity>
    {
        public void Configure(EntityTypeBuilder<VehicleEntity> builder)
        {
            builder.ToTable(DbSchemaConstants.Vehicle);
            builder.HasIndex(x => x.LicensePlate).IsUnique();
        }
    }
}