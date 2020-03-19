using Microsoft.EntityFrameworkCore;
using MockSchoolManagement.Models;
using MockSchoolManagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                 new Student
                 {
                     Id = 2,
                     Name = "张三",
                     Major = MajorEnum.ComputerScience,
                     Email = "zhangsan@52abp.com"
                 }
             );
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 3,
                    Name = "李四",
                    Major = MajorEnum.Mathematics,
                    Email = "lisi@52abp.com"
                }
            );


            ///指定实体在数据库中生成的名称。
            modelBuilder.Entity<Course>().ToTable("Course", "School");
            modelBuilder.Entity<StudentCourse>().ToTable("StudentCourse");
            modelBuilder.Entity<Student>().ToTable("Student");


        }
    }
}
