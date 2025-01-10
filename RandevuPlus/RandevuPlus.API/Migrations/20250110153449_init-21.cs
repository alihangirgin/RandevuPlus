using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandevuPlus.API.Migrations
{
    /// <inheritdoc />
    public partial class init21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Message",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
