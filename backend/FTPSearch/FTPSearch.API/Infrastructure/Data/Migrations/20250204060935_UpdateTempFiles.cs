using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTPSearch.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTempFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_TempFiles_Name_Path_SiteId",
                table: "TempFiles",
                newName: "IX_TempFiles_Name_Path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_TempFiles_Name_Path",
                table: "TempFiles",
                newName: "IX_TempFiles_Name_Path_SiteId");
        }
    }
}
