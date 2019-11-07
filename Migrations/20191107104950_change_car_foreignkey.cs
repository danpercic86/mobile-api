using Microsoft.EntityFrameworkCore.Migrations;

namespace itec_mobile_api_final.Migrations
{
    public partial class change_car_foreignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CarEntities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarEntities_UserId",
                table: "CarEntities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntities_AspNetUsers_UserId",
                table: "CarEntities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarEntities_AspNetUsers_UserId",
                table: "CarEntities");

            migrationBuilder.DropIndex(
                name: "IX_CarEntities_UserId",
                table: "CarEntities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CarEntities");
        }
    }
}
