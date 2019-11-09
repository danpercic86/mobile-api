using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryForumEntities",
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
                    table.PrimaryKey("PK_CategoryForumEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryForumEntities_CategoryForumEntities_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryForumEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryForumEntities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopicForumEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    Desctiption = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicForumEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicForumEntities_CategoryForumEntities_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryForumEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicForumEntities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageForumEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    TopicId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageForumEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageForumEntities_TopicForumEntities_TopicId",
                        column: x => x.TopicId,
                        principalTable: "TopicForumEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageForumEntities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryForumEntities_CategoryId",
                table: "CategoryForumEntities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryForumEntities_UserId",
                table: "CategoryForumEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageForumEntities_TopicId",
                table: "MessageForumEntities",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageForumEntities_UserId",
                table: "MessageForumEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicForumEntities_CategoryId",
                table: "TopicForumEntities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicForumEntities_UserId",
                table: "TopicForumEntities",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageForumEntities");

            migrationBuilder.DropTable(
                name: "TopicForumEntities");

            migrationBuilder.DropTable(
                name: "CategoryForumEntities");
        }
    }
}
