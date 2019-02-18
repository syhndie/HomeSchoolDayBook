using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSchoolDayBook.Data.Migrations
{
    public partial class addedEmailConfirmsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailConfirmsCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmsCount",
                table: "AspNetUsers");
        }
    }
}
