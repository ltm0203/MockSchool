using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MockSchoolManagement.Migrations
{
    public partial class Remove_SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "StudentCourse",
                newName: "StudentCourse",
                newSchema: "School");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Student",
                newSchema: "School");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "StudentCourse",
                schema: "School",
                newName: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "Student",
                schema: "School",
                newName: "Student");

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "Email", "EnrollmentDate", "Major", "Name", "PhotoPath" },
                values: new object[] { 2, "zhangsan@52abp.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "张三", null });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "Email", "EnrollmentDate", "Major", "Name", "PhotoPath" },
                values: new object[] { 3, "lisi@52abp.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "李四", null });
        }
    }
}
