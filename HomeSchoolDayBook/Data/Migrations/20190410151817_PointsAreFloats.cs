using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSchoolDayBook.Data.Migrations
{
    public partial class PointsAreFloats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PointsEarned",
                table: "Grades",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "PointsAvailable",
                table: "Grades",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PointsEarned",
                table: "Grades",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "PointsAvailable",
                table: "Grades",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
