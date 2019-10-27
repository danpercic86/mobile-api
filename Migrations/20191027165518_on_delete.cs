using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class on_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sockets",
                schema: "mobile-api",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    StationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sockets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sockets_Stations_StationId",
                        column: x => x.StationId,
                        principalSchema: "mobile-api",
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sockets_StationId",
                schema: "mobile-api",
                table: "Sockets",
                column: "StationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sockets",
                schema: "mobile-api");
        }
    }
}
