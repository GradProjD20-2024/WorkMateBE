using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkMateBE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFaceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FaceId",
                table: "Accounts",
                newName: "Face_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Face_id",
                table: "Accounts",
                newName: "FaceId");
        }
    }
}
