using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class delete_cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sockets_Stations_StationEntityId",
                schema: "mobile-api",
                table: "Sockets");

            migrationBuilder.RenameColumn(
                name: "StationEntityId",
                schema: "mobile-api",
                table: "Sockets",
                newName: "StationId");

            migrationBuilder.RenameIndex(
                name: "IX_Sockets_StationEntityId",
                schema: "mobile-api",
                table: "Sockets",
                newName: "IX_Sockets_StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sockets_Stations_StationId",
                schema: "mobile-api",
                table: "Sockets",
                column: "StationId",
                principalSchema: "mobile-api",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sockets_Stations_StationId",
                schema: "mobile-api",
                table: "Sockets");

            migrationBuilder.RenameColumn(
                name: "StationId",
                schema: "mobile-api",
                table: "Sockets",
                newName: "StationEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Sockets_StationId",
                schema: "mobile-api",
                table: "Sockets",
                newName: "IX_Sockets_StationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sockets_Stations_StationEntityId",
                schema: "mobile-api",
                table: "Sockets",
                column: "StationEntityId",
                principalSchema: "mobile-api",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
