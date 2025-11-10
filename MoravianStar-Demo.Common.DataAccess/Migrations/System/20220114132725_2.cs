using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoravianStar_Demo.Common.DataAccess.Migrations.System
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Client ([Name], Status) values ('You shall pass!', 1)
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
