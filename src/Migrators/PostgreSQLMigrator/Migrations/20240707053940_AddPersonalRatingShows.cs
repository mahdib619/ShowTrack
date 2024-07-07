﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresqlMigrator.Migrations
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
                type: "integer",
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
