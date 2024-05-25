using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresqlMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddShowsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shows",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(36)", unicode: false, maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", unicode: false, maxLength: 36, nullable: false),
                    CurrentSeason = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ScheduleId = table.Column<string>(type: "character varying(36)", unicode: false, maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shows_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowSchedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(36)", unicode: false, maxLength: 36, nullable: false),
                    ShowId = table.Column<string>(type: "character varying(36)", unicode: false, maxLength: 36, nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Season = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowSchedules_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shows_UserId",
                table: "Shows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowSchedules_ShowId",
                table: "ShowSchedules",
                column: "ShowId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowSchedules");

            migrationBuilder.DropTable(
                name: "Shows");
        }
    }
}
