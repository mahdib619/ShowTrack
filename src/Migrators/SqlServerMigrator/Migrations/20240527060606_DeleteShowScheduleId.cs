using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerMigrator.Migrations
{
    /// <inheritdoc />
    public partial class DeleteShowScheduleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Shows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScheduleId",
                table: "Shows",
                type: "varchar(36)",
                unicode: false,
                maxLength: 36,
                nullable: true);
        }
    }
}
