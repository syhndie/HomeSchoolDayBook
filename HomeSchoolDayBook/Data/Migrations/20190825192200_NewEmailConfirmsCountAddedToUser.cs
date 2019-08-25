using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSchoolDayBook.Data.Migrations
{
    public partial class NewEmailConfirmsCountAddedToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewEmailConfirmsCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewEmailConfirmsCount",
                table: "AspNetUsers");
        }
    }
}
