using Microsoft.EntityFrameworkCore.Migrations;

namespace TheatreWebApp.Data.Migrations
{
    public partial class PlaysFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plays_Stages_StageId",
                table: "Plays");

            migrationBuilder.DropIndex(
                name: "IX_Plays_StageId",
                table: "Plays");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Plays");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StageId",
                table: "Plays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plays_StageId",
                table: "Plays",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plays_Stages_StageId",
                table: "Plays",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
