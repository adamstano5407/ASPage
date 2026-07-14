using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIKros.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHierarchyModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Employees_DirectorId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "DirectorId",
                table: "Companies",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_DirectorId",
                table: "Companies",
                newName: "IX_Companies_ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Employees_ManagerId",
                table: "Companies",
                column: "ManagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Employees_ManagerId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Companies",
                newName: "DirectorId");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_ManagerId",
                table: "Companies",
                newName: "IX_Companies_DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Employees_DirectorId",
                table: "Companies",
                column: "DirectorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
