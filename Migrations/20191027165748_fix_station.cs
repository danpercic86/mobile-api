using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class fix_station : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sockets",
                schema: "mobile-api");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sockets",
                schema: "mobile-api",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    StationEntityId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sockets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sockets_Stations_StationEntityId",
                        column: x => x.StationEntityId,
                        principalSchema: "mobile-api",
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sockets_StationEntityId",
                schema: "mobile-api",
                table: "Sockets",
                column: "StationEntityId");
        }
    }
}
