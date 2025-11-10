using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using MoravianStar_Demo.Common.Core.Resources;
using MoravianStar_Demo.Common.DataAccess.Constants;
using System;

namespace MoravianStar_Demo.Common.DataAccess.Extensions
{
    public static class MigrationBuilderExtensions
    {
        public static OperationBuilder<SqlOperation> CreateSynonym(this MigrationBuilder migrationBuilder, string name)
        {
            OperationBuilder<SqlOperation> result;

            switch (migrationBuilder.ActiveProvider)
            {
                case EFCoreProviderConstants.MicrosoftEFCoreSqlServer:
                    {
                        result = migrationBuilder.Sql($"CREATE SYNONYM [dbo].[{name}] FOR [MoravianStarDemo_System].[dbo].[{name}]");
                        break;
                    }
                default:
                    throw new ArgumentException(Strings.UnknownDatabaseProvider, nameof(migrationBuilder.ActiveProvider));
            }

            return result;
        }

        public static OperationBuilder<SqlOperation> DropSynonym(this MigrationBuilder migrationBuilder, string name)
        {
            OperationBuilder<SqlOperation> result;

            switch (migrationBuilder.ActiveProvider)
            {
                case EFCoreProviderConstants.MicrosoftEFCoreSqlServer:
                    {
                        result = migrationBuilder.Sql($"DROP SYNONYM [dbo].[{name}]");
                        break;
                    }
                default:
                    throw new ArgumentException(Strings.UnknownDatabaseProvider, nameof(migrationBuilder.ActiveProvider));
            }

            return result;
        }
    }
}