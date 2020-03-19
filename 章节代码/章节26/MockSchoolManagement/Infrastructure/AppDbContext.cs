

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MockSchoolManagement.Models;
using System.Linq;

namespace MockSchoolManagement.Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        //注意:将ApplicationUser作为泛型参数传递给IdentityDbContext类

        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            //获取当前系统中所有领域模型上的外键列表
        var foreignKeys=    modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var foreignKey in foreignKeys)
            {
                //然后将它们的删除行为配置为 Restrict即无操作
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}
