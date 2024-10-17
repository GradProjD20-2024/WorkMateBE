using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkMateBE.Migrations
{
    /// <inheritdoc />
    public partial class AddLateStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Late",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Late",
                table: "Attendances");
        }
    }
}
