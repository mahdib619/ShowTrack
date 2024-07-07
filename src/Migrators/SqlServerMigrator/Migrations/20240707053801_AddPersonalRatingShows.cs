using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalRatingShows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonalRating",
                table: "Shows",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalRating",
                table: "Shows");
        }
    }
}
