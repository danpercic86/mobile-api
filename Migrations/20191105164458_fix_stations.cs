using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class fix_stations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationStr",
                schema: "mobile-api",
                table: "Stations");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "mobile-api",
                table: "Stations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "mobile-api",
                table: "Stations");

            migrationBuilder.AddColumn<string>(
                name: "LocationStr",
                schema: "mobile-api",
                table: "Stations",
                nullable: true);
        }
    }
}
