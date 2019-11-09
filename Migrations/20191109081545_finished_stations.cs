using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class finished_stations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OccupiedSockets",
                table: "StationEntities",
                newName: "FreeSockets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FreeSockets",
                table: "StationEntities",
                newName: "OccupiedSockets");
        }
    }
}
