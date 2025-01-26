using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTPSearch.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Path_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FileEntity_Path",
                table: "Files",
                column: "Path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileEntity_Path",
                table: "Files");
        }
    }
}
