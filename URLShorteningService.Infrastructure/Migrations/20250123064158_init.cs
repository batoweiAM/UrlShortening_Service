using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShorteningService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    LongUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlMappingId = table.Column<int>(type: "int", nullable: false),
                    AccessCount = table.Column<int>(type: "int", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlStats_UrlMappings_UrlMappingId",
                        column: x => x.UrlMappingId,
                        principalTable: "UrlMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlMappings_ShortCode",
                table: "UrlMappings",
                column: "ShortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlStats_UrlMappingId",
                table: "UrlStats",
                column: "UrlMappingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlStats");

            migrationBuilder.DropTable(
                name: "UrlMappings");
        }
    }
}
