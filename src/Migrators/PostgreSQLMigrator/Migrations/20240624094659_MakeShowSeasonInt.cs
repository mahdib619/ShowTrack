using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresqlMigrator.Migrations
{
    /// <inheritdoc />
    public partial class MakeShowSeasonInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""ShowSchedules"" ALTER COLUMN ""Season"" TYPE integer USING (Season::integer);");
            migrationBuilder.Sql(@"ALTER TABLE ""Shows"" ALTER COLUMN ""CurrentSeason"" TYPE integer USING (Season::integer);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Season",
                table: "ShowSchedules",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentSeason",
                table: "Shows",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 10);
        }
    }
}
