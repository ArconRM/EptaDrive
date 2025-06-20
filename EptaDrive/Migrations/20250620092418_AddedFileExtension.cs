using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EptaDrive.Migrations
{
    /// <inheritdoc />
    public partial class AddedFileExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "UserFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "UserFiles");
        }
    }
}
