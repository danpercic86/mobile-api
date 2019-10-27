using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class Stations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Stations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocketsEntity",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    StationEntityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocketsEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocketsEntity_Stations_StationEntityId",
                        column: x => x.StationEntityId,
                        principalSchema: "dbo",
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocketsEntity_StationEntityId",
                table: "SocketsEntity",
                column: "StationEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocketsEntity");

            migrationBuilder.DropTable(
                name: "Stations",
                schema: "dbo");
        }
    }
}
