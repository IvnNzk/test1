using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace upload_backend.Migrations
{
    public partial class create_table_dataset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dataset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ImagesCount = table.Column<int>(type: "integer", nullable: false),
                    ContainCyrillic = table.Column<bool>(type: "boolean", nullable: false),
                    ContainsNumbers = table.Column<bool>(type: "boolean", nullable: false),
                    ContainSpecialCharacters = table.Column<bool>(type: "boolean", nullable: false),
                    CaseSensitivity = table.Column<bool>(type: "boolean", nullable: false),
                    AnswersType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dataset", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dataset");
        }
    }
}
