using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedConfirmationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmployeeConfirmed",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeConfirmed",
                table: "Employees");
        }
    }
}
