using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoravianStar_Demo.Common.DataAccess.Migrations.System
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Client ([Name], Status) values ('You shall pass (Migration 4)!', 0)
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
