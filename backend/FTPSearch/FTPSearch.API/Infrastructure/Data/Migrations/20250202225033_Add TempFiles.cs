using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTPSearch.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTempFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileEntity_Name_Path",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TempFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    FileMetaType = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileEntity_Name_GIN",
                table: "Files",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "IX_FileEntity_Name_Path",
                table: "Files",
                columns: new[] { "Name", "Path", "SiteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TempFiles_Name_Path_SiteId",
                table: "TempFiles",
                columns: new[] { "Name", "Path", "SiteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TempFiles_Path",
                table: "TempFiles",
                column: "Path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempFiles");

            migrationBuilder.DropIndex(
                name: "IX_FileEntity_Name_GIN",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_FileEntity_Name_Path",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Files");

            migrationBuilder.CreateIndex(
                name: "IX_FileEntity_Name_Path",
                table: "Files",
                columns: new[] { "Name", "Path" },
                unique: true);
        }
    }
}
