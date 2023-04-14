using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Persistence.Attributes;
using MoravianStar_Demo.Persistence.Constants;
using MoravianStar_Demo.Persistence.DbContexts;

namespace MoravianStar_Demo.Persistence.EntityConfigurations.Test
{
    [ForDbContext(typeof(DataLayer_ClientContext))]
    public class BlockEntityConfiguration : IEntityTypeConfiguration<BlockEntity>
    {
        public void Configure(EntityTypeBuilder<BlockEntity> builder)
        {
            builder.ToTable(DbSchemaConstants.Block);
            builder.Property(x => x.Boundaries).HasColumnType("geography");
        }
    }
}