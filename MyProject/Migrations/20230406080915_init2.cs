using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_DepartmentID",
                table: "Tbl_Employee");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Tbl_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_DepartmentID",
                table: "Tbl_Employee",
                column: "DepartmentID",
                principalTable: "Tbl_Department",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_DepartmentID",
                table: "Tbl_Employee");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Tbl_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_Department_DepartmentID",
                table: "Tbl_Employee",
                column: "DepartmentID",
                principalTable: "Tbl_Department",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
