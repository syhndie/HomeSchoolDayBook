using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeSchoolDayBook.Data.Migrations
{
    public partial class RenameGradeTableToGrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Entries_EntryID",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Students_StudentID",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Subjects_SubjectID",
                table: "Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grade",
                table: "Grade");

            migrationBuilder.RenameTable(
                name: "Grade",
                newName: "Grades");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_SubjectID",
                table: "Grades",
                newName: "IX_Grades_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_StudentID",
                table: "Grades",
                newName: "IX_Grades_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_EntryID",
                table: "Grades",
                newName: "IX_Grades_EntryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Entries_EntryID",
                table: "Grades",
                column: "EntryID",
                principalTable: "Entries",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentID",
                table: "Grades",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Subjects_SubjectID",
                table: "Grades",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Entries_EntryID",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentID",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Subjects_SubjectID",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.RenameTable(
                name: "Grades",
                newName: "Grade");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_SubjectID",
                table: "Grade",
                newName: "IX_Grade_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_StudentID",
                table: "Grade",
                newName: "IX_Grade_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_EntryID",
                table: "Grade",
                newName: "IX_Grade_EntryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grade",
                table: "Grade",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Entries_EntryID",
                table: "Grade",
                column: "EntryID",
                principalTable: "Entries",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Students_StudentID",
                table: "Grade",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Subjects_SubjectID",
                table: "Grade",
                column: "SubjectID",
                principalTable: "Subjects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
