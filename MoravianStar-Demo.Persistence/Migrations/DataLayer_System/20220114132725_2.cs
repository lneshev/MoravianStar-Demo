using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Persistence.Migrations.DataLayer_System
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Client ([Name], Status) values ('You shall pass!', 1)
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
