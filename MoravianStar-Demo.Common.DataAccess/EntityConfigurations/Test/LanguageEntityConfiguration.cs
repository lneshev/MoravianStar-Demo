using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Common.DataAccess.Attributes;
using MoravianStar_Demo.Common.DataAccess.Constants;
using MoravianStar_Demo.Common.DataAccess.DbContexts;

namespace MoravianStar_Demo.Common.DataAccess.EntityConfigurations.Test
{
    [ForDbContext(typeof(SystemContext))]
    public class LanguageEntityConfiguration : IEntityTypeConfiguration<LanguageEntity>
    {
        public void Configure(EntityTypeBuilder<LanguageEntity> builder)
        {
            builder.ToTable(DbSchemaConstants.Language);
        }
    }
}