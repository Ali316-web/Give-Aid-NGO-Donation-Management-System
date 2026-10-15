using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Give_Aid.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AdminContacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedDate",
                table: "AdminContacts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reply",
                table: "AdminContacts",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AdminContacts");

            migrationBuilder.DropColumn(
                name: "RepliedDate",
                table: "AdminContacts");

            migrationBuilder.DropColumn(
                name: "Reply",
                table: "AdminContacts");
        }
    }
}
