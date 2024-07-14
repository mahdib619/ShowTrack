﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresqlMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddDatePinnedShows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePinned",
                table: "Shows",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePinned",
                table: "Shows");
        }
    }
}
