using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Infrastructure.EntityMapper;
using MockSchoolManagement.Models;
using MockSchoolManagement.Models.BlogManagement;
using MockSchoolManagement.Models.Blogs;
using System.Linq;

namespace MockSchoolManagement.EntityFrameworkCore
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        //注意:将ApplicationUser作为泛型参数传递给IdentityDbContext类

        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<OfficeLocation> OfficeLocations { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            //获取当前系统中所有领域模型上的外键列表
            var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var foreignKey in foreignKeys)
            {
                //然后将它们的删除行为配置为 Restrict即无操作
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.ApplyConfiguration(new BlogMapper());
            modelBuilder.ApplyConfiguration(new PostMapper());
            modelBuilder.ApplyConfiguration(new StudentCourseMapper());
        }
    }
}