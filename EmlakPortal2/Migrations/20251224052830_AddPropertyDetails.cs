using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmlakPortal2.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BalkonSayisi",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BinaYasi",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BulunduguKat",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EsyaliMi",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IsitmaTipi",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "KatSayisi",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KullanimDurumu",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SiteIcerisinde",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalkonSayisi",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "BinaYasi",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "BulunduguKat",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "EsyaliMi",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "IsitmaTipi",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "KatSayisi",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "KullanimDurumu",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SiteIcerisinde",
                table: "Properties");
        }
    }
}
