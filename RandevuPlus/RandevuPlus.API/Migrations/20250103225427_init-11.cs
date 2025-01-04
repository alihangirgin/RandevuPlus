using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandevuPlus.API.Migrations
{
    /// <inheritdoc />
    public partial class init11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Instructors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
