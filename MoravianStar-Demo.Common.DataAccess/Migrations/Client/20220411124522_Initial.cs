using Microsoft.EntityFrameworkCore.Migrations;
using MoravianStar_Demo.Common.DataAccess.Extensions;
using NetTopologySuite.Geometries;

#nullable disable

namespace MoravianStar_Demo.Common.DataAccess.Migrations.Client
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSynonym("Client");

            migrationBuilder.CreateTable(
                name: "Block",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    Boundaries = table.Column<Polygon>(type: "geography", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Block_ClientId",
                table: "Block",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Block");

            migrationBuilder.DropSynonym("Client");
        }
    }
}
