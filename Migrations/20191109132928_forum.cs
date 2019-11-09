using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class forum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    LastTechRevision = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryEntities_CategoryEntities_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryEntities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopicEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicEntities_CategoryEntities_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicEntities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TopicId = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageEntities_TopicEntities_TopicId",
                        column: x => x.TopicId,
                        principalTable: "TopicEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageEntities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEntities_CategoryId",
                table: "CategoryEntities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEntities_UserId",
                table: "CategoryEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEntities_TopicId",
                table: "MessageEntities",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEntities_UserId",
                table: "MessageEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicEntities_CategoryId",
                table: "TopicEntities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicEntities_UserId",
                table: "TopicEntities",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageEntities");

            migrationBuilder.DropTable(
                name: "TopicEntities");

            migrationBuilder.DropTable(
                name: "CategoryEntities");
        }
    }
}
