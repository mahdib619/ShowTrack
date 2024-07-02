using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddDateAddedShows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [Shows] ADD [DateAdded] datetime2 NULL;");

            migrationBuilder.Sql("UPDATE [Shows] SET [DateAdded] = GETDATE();");

            migrationBuilder.Sql("""
                                 ALTER TABLE [Shows]
                                 ALTER COLUMN [DateAdded] datetime2 NOT NULL;
                                 """);
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
