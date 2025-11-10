using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.DataAccess.Attributes;
using MoravianStar_Demo.Common.DataAccess.Constants;
using MoravianStar_Demo.Common.DataAccess.DbContexts;

namespace MoravianStar_Demo.Common.DataAccess.EntityConfigurations.Test
{
    [ForDbContext(typeof(ClientContext))]
    public class BlockEntityConfiguration : IEntityTypeConfiguration<BlockEntity>
    {
        public void Configure(EntityTypeBuilder<BlockEntity> builder)
        {
            builder.ToTable(DbSchemaConstants.Block);
            builder.Property(x => x.Boundaries).HasColumnType("geography");
        }
    }
}