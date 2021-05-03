using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherStationWebAPI.Migrations
{
    public partial class AttachedLogRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_WeatherLogs_LogId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_LogId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "LogId",
                table: "Places");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LogId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_LogId",
                table: "Places",
                column: "LogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_WeatherLogs_LogId",
                table: "Places",
                column: "LogId",
                principalTable: "WeatherLogs",
                principalColumn: "LogId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
