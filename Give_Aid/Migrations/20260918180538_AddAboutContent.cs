using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Give_Aid.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AboutSections",
                columns: new[] { "Id", "Content", "IsVisible", "SectionKey", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "<h2>Connecting resources to where they matter most</h2><p>We vet and onboard associated NGOs, run transparent donation pipelines by cause, and organize joint programmes across healthcare, education and welfare — so donors can trust exactly where their contribution lands.</p>", true, "what-we-do", "What We Do", new DateTime(2026, 9, 18, 23, 5, 37, 251, DateTimeKind.Local).AddTicks(824) },
                    { 2, "<h2>Shelter, dignity and opportunity for every community we reach</h2><p>We believe lasting change grows from local roots. Our mission is to strengthen grassroots NGOs with funding, visibility and shared infrastructure — not to replace them.</p>", true, "mission", "Our Mission", new DateTime(2026, 9, 18, 23, 5, 37, 252, DateTimeKind.Local).AddTicks(7097) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutSections");

            migrationBuilder.DropTable(
                name: "TeamMembers");
        }
    }
}
