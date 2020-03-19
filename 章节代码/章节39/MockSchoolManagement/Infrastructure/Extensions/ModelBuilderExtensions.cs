using Microsoft.AspNetCore.Identity;
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
            ///指定实体在数据库中生成的名称。
            modelBuilder.Entity<Course>().ToTable("Course", "School");
            modelBuilder.Entity<StudentCourse>().ToTable("StudentCourse", "School"); 
            modelBuilder.Entity<Person>().ToTable("Person");                                

            modelBuilder.Entity<CourseAssignment>()
                   .HasKey(c => new { c.CourseID, c.TeacherID });
        }
    }
}
