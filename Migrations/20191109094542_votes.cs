using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class votes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoteEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Vote = table.Column<bool>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    StationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteEntities_StationEntities_StationId",
                        column: x => x.StationId,
                        principalTable: "StationEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoteEntities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoteEntities_StationId",
                table: "VoteEntities",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteEntities_UserId",
                table: "VoteEntities",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoteEntities");
        }
    }
}
