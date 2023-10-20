using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoravianStar_Demo.Persistence.Migrations.System
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                --INSERT INTO Client ([Name], Status) values ('You shall not pass! Because it is loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooonger than 100 chars.', 1)
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
