using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSchoolDayBook.Data.Migrations
{
    public partial class StudentUserIDToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Students",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
