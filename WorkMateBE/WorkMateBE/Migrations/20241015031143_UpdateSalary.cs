using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkMateBE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryMonth",
                table: "Salaries");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Salaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Salaries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Salaries");

            migrationBuilder.AddColumn<DateTime>(
                name: "SalaryMonth",
                table: "Salaries",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
