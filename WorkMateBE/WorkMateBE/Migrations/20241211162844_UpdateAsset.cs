using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkMateBE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Employees_EmployeeId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_EmployeeId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Assets",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Assets",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantiy",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Quantiy",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Assets",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Assets",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Assets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_EmployeeId",
                table: "Assets",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Employees_EmployeeId",
                table: "Assets",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
