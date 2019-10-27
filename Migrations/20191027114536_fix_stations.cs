using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class fix_stations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mobile-api");

            migrationBuilder.RenameTable(
                name: "Stations",
                schema: "dbo",
                newName: "Stations",
                newSchema: "mobile-api");

            migrationBuilder.RenameTable(
                name: "Sockets",
                schema: "dbo",
                newName: "Sockets",
                newSchema: "mobile-api");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Stations",
                schema: "mobile-api",
                newName: "Stations",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Sockets",
                schema: "mobile-api",
                newName: "Sockets",
                newSchema: "dbo");
        }
    }
}
