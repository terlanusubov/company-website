using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelApp.Migrations
{
    public partial class SettingChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "AboutLanguages");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Settings",
                newName: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Settings",
                newName: "Logo");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AboutLanguages",
                nullable: true);
        }
    }
}
