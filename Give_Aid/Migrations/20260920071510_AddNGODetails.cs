using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Give_Aid.Migrations
{
    /// <inheritdoc />
    public partial class AddNGODetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AssociatedNGOs",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssociatedNGOs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AssociatedNGOs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "AssociatedNGOs",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AssociatedNGOs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "AssociatedNGOs",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AssociatedNGOs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssociatedNGOs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AssociatedNGOs");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "AssociatedNGOs");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AssociatedNGOs");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "AssociatedNGOs");
        }
    }
}
