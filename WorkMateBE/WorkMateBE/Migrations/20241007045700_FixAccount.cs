using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkMateBE.Migrations
{
    /// <inheritdoc />
    public partial class FixAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role",
                table: "Accounts",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Accounts",
                newName: "role");
        }
    }
}
