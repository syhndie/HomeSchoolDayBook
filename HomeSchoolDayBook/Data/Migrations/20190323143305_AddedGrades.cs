using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HomeSchoolDayBook.Data.Migrations
{
    public partial class AddedGrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EntryID = table.Column<int>(nullable: false),
                    StudentID = table.Column<int>(nullable: false),
                    SubjectID = table.Column<int>(nullable: false),
                    PointsEarned = table.Column<int>(nullable: false),
                    PointsAvailable = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Grade_Entries_EntryID",
                        column: x => x.EntryID,
                        principalTable: "Entries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grade_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grade_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grade_EntryID",
                table: "Grade",
                column: "EntryID");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_StudentID",
                table: "Grade",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_SubjectID",
                table: "Grade",
                column: "SubjectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grade");
        }
    }
}
