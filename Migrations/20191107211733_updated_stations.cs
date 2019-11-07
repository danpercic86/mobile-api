using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class updated_stations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Old",
                table: "StationEntities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OldStationId",
                table: "StationEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "StationEntities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationEntities_OldStationId",
                table: "StationEntities",
                column: "OldStationId");

            migrationBuilder.CreateIndex(
                name: "IX_StationEntities_UserId",
                table: "StationEntities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StationEntities_StationEntities_OldStationId",
                table: "StationEntities",
                column: "OldStationId",
                principalTable: "StationEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StationEntities_AspNetUsers_UserId",
                table: "StationEntities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationEntities_StationEntities_OldStationId",
                table: "StationEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_StationEntities_AspNetUsers_UserId",
                table: "StationEntities");

            migrationBuilder.DropIndex(
                name: "IX_StationEntities_OldStationId",
                table: "StationEntities");

            migrationBuilder.DropIndex(
                name: "IX_StationEntities_UserId",
                table: "StationEntities");

            migrationBuilder.DropColumn(
                name: "Old",
                table: "StationEntities");

            migrationBuilder.DropColumn(
                name: "OldStationId",
                table: "StationEntities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StationEntities");
        }
    }
}
