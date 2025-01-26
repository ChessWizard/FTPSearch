using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTPSearch.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileMetaType_Added_to_FileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileMetaType",
                table: "Files",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileMetaType",
                table: "Files");
        }
    }
}
