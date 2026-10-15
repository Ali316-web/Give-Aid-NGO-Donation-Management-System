using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Give_Aid.Migrations
{
    /// <inheritdoc />
    public partial class AddCauseDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "DonationCauses",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DonationCauses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Goal",
                table: "DonationCauses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "DonationCauses",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DonationCauses");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "DonationCauses");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "DonationCauses");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "DonationCauses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
