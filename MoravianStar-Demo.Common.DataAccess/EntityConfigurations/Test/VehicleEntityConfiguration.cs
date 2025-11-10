using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.DataAccess.Attributes;
using MoravianStar_Demo.Common.DataAccess.Constants;
using MoravianStar_Demo.Common.DataAccess.DbContexts;

namespace MoravianStar_Demo.Common.DataAccess.EntityConfigurations.Test
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