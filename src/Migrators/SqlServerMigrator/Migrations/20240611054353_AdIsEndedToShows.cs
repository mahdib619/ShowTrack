using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AdIsEndedToShows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "Shows",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "Shows");
        }
    }
}
