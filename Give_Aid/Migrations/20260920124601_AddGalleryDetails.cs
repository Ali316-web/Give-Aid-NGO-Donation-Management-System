using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Give_Aid.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Gallery",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProgrammeId",
                table: "Gallery",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_ProgrammeId",
                table: "Gallery",
                column: "ProgrammeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gallery_Programmers_ProgrammeId",
                table: "Gallery",
                column: "ProgrammeId",
                principalTable: "Programmers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gallery_Programmers_ProgrammeId",
                table: "Gallery");

            migrationBuilder.DropIndex(
                name: "IX_Gallery_ProgrammeId",
                table: "Gallery");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Gallery");

            migrationBuilder.DropColumn(
                name: "ProgrammeId",
                table: "Gallery");
        }
    }
}
