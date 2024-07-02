using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresqlMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddDateAddedShows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Shows"" ADD ""DateAdded"" timestamp with time zone NULL;");

            migrationBuilder.Sql(@"UPDATE ""Shows"" SET ""DateAdded"" = now();");

            migrationBuilder.Sql(@"ALTER TABLE ""Shows"" 
                                   ALTER COLUMN ""DateAdded"" SET NOT NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Shows");
        }
    }
}
