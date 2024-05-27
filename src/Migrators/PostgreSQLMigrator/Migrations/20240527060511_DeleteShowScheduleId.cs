using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresqlMigrator.Migrations
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
                type: "character varying(36)",
                unicode: false,
                maxLength: 36,
                nullable: true);
        }
    }
}
