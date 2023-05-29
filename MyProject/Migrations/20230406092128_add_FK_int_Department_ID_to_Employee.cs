using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Migrations
{
    /// <inheritdoc />
    public partial class addFKintDepartmentIDtoEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_DepartmentID",
                table: "Tbl_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Employee_DepartmentID",
                table: "Tbl_Employee");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Tbl_Employee");

            migrationBuilder.AddColumn<int>(
                name: "Department_ID",
                table: "Tbl_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Employee_Department_ID",
                table: "Tbl_Employee",
                column: "Department_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_Department_ID",
                table: "Tbl_Employee",
                column: "Department_ID",
                principalTable: "Tbl_Department",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_Department_ID",
                table: "Tbl_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Employee_Department_ID",
                table: "Tbl_Employee");

            migrationBuilder.DropColumn(
                name: "Department_ID",
                table: "Tbl_Employee");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Tbl_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Employee_DepartmentID",
                table: "Tbl_Employee",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_DepartmentID",
                table: "Tbl_Employee",
                column: "DepartmentID",
                principalTable: "Tbl_Department",
                principalColumn: "ID");
        }
    }
}
