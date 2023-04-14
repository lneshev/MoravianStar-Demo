using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoravianStar_Demo.Common.Core.Entities.Test;
using MoravianStar_Demo.Persistence.Attributes;
using MoravianStar_Demo.Persistence.Constants;
using MoravianStar_Demo.Persistence.DbContexts;

namespace MoravianStar_Demo.Persistence.EntityConfigurations.Test
{
    [ForDbContext(typeof(DataLayer_SystemContext))]
    public class LanguageEntityConfiguration : IEntityTypeConfiguration<LanguageEntity>
    {
        public void Configure(EntityTypeBuilder<LanguageEntity> builder)
        {
            builder.ToTable(DbSchemaConstants.Language);
        }
    }
}