using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MockSchoolManagement.Migrations
{
    public partial class AddPersonEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Teachers_TeacherID",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Teachers_TeacherID",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeLocations_Teachers_TeacherId",
                table: "OfficeLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Student_StudentID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                schema: "School",
                table: "Student");

            migrationBuilder.RenameTable(
                name: "Student",
                schema: "School",
                newName: "Person");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Person",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Person",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Person",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Person",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Person_TeacherID",
                table: "CourseAssignments",
                column: "TeacherID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Person_TeacherID",
                table: "Departments",
                column: "TeacherID",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeLocations_Person_TeacherId",
                table: "OfficeLocations",
                column: "TeacherId",
                principalTable: "Person",
                principalColumn: "Id",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignments_Person_TeacherID",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Person_TeacherID",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeLocations_Person_TeacherId",
                table: "OfficeLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Person_StudentID",
                schema: "School",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Student",
                newSchema: "School");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDate",
                schema: "School",
                table: "Student",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "School",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                schema: "School",
                table: "Student",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_Teachers_TeacherID",
                table: "CourseAssignments",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Teachers_TeacherID",
                table: "Departments",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeLocations_Teachers_TeacherId",
                table: "OfficeLocations",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Student_StudentID",
                schema: "School",
                table: "StudentCourse",
                column: "StudentID",
                principalSchema: "School",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
