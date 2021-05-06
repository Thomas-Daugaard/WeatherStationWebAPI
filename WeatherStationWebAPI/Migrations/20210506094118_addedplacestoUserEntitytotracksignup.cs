using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherStationWebAPI.Migrations
{
    public partial class addedplacestoUserEntitytotracksignup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PlaceId",
                table: "WeatherLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PlaceId",
                table: "Places",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Users_PlaceId",
                table: "Places",
                column: "PlaceId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Users_PlaceId",
                table: "Places");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "WeatherLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "Places",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
