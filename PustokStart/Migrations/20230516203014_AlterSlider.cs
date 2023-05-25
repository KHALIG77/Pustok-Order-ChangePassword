using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PustokStart.Migrations
{
    public partial class AlterSlider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Slides");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Slides",
                newName: "Title2");

            migrationBuilder.RenameColumn(
                name: "BgColor",
                table: "Slides",
                newName: "Title1");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BtnText",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BtnUrl",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Slides",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BtnText",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "BtnUrl",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Slides");

            migrationBuilder.RenameColumn(
                name: "Title2",
                table: "Slides",
                newName: "Desc");

            migrationBuilder.RenameColumn(
                name: "Title1",
                table: "Slides",
                newName: "BgColor");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Slides",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
