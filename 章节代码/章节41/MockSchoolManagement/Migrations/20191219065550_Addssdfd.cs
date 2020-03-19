using Microsoft.EntityFrameworkCore.Migrations;

namespace MockSchoolManagement.Migrations
{
    public partial class Addssdfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Course_CourseID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Person_StudentID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourse",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "StudentCourse",
                schema: "School",
                newName: "Enrollment",
                newSchema: "School");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_StudentID",
                schema: "School",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_CourseID",
                schema: "School",
                table: "Enrollment",
                newName: "IX_Enrollment_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollment",
                schema: "School",
                table: "Enrollment",
                column: "StudentCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                schema: "School",
                table: "Enrollment",
                column: "CourseID",
                principalSchema: "School",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Person_StudentID",
                schema: "School",
                table: "Enrollment",
                column: "StudentID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                schema: "School",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Person_StudentID",
                schema: "School",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollment",
                schema: "School",
                table: "Enrollment");

            migrationBuilder.RenameTable(
                name: "Enrollment",
                schema: "School",
                newName: "StudentCourse",
                newSchema: "School");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentID",
                schema: "School",
                table: "StudentCourse",
                newName: "IX_StudentCourse_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CourseID",
                schema: "School",
                table: "StudentCourse",
                newName: "IX_StudentCourse_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourse",
                schema: "School",
                table: "StudentCourse",
                column: "StudentCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Course_CourseID",
                schema: "School",
                table: "StudentCourse",
                column: "CourseID",
                principalSchema: "School",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Person_StudentID",
                schema: "School",
                table: "StudentCourse",
                column: "StudentID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
