using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherStationWebAPI.Migrations
{
    public partial class ForeignKeyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Places_LogId",
                table: "Places");

            migrationBuilder.RenameColumn(
                name: "AirMoisture",
                table: "WeatherLogs",
                newName: "Humidity");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "WeatherLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherLogs_PlaceId",
                table: "WeatherLogs",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_LogId",
                table: "Places",
                column: "LogId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherLogs_Places_PlaceId",
                table: "WeatherLogs",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherLogs_Places_PlaceId",
                table: "WeatherLogs");

            migrationBuilder.DropIndex(
                name: "IX_WeatherLogs_PlaceId",
                table: "WeatherLogs");

            migrationBuilder.DropIndex(
                name: "IX_Places_LogId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "WeatherLogs");

            migrationBuilder.RenameColumn(
                name: "Humidity",
                table: "WeatherLogs",
                newName: "AirMoisture");

            migrationBuilder.CreateIndex(
                name: "IX_Places_LogId",
                table: "Places",
                column: "LogId",
                unique: true,
                filter: "[LogId] IS NOT NULL");
        }
    }
}
