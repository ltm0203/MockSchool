using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MockSchoolManagement.Models;
using MockSchoolManagement.Models.EnumTypes;
using System;
using System.Linq;

namespace MockSchoolManagement.Infrastructure.Data
{
    public static class DataInitializer
    {
        public static IApplicationBuilder UseDataInitializer(
       this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                #region 学生种子信息

                if (dbcontext.Students.Any())
                {
                    return builder; // 数据已经初始化了。
                }

                var students = new[] {
              new Student { Name = "张三", Major = MajorEnum.ComputerScience, Email = "zhangsan@52abp.com", EnrollmentDate = DateTime.Parse ("2016-09-01"), },
              new Student { Name = "李四", Major = MajorEnum.Mathematics, Email = "lisi@52abp.com", EnrollmentDate = DateTime.Parse ("2017-09-01") },
              new Student { Name = "王五", Major = MajorEnum.ElectronicCommerce, Email = "wangwu@52abp.com", EnrollmentDate = DateTime.Parse ("2012-09-01") }
          };
                foreach (Student item in students)
                {
                    dbcontext.Students.Add(item);
                }
                dbcontext.SaveChanges();

                #endregion 学生种子信息

                #region 课程种子数据

                if (dbcontext.Courses.Any())
                {
                    return builder; // 数据已经初始化了。
                }
                var courses = new[] {
              new Course { CourseID = 1050, Title = "数学", Credits = 3, },
              new Course { CourseID = 4022, Title = "政治", Credits = 3, },
              new Course { CourseID = 4041, Title = "物理", Credits = 3, },
              new Course { CourseID = 1045, Title = "化学", Credits = 4, },
              new Course { CourseID = 3141, Title = "生物", Credits = 4, },
              new Course { CourseID = 2021, Title = "英语", Credits = 3, },
              new Course { CourseID = 2042, Title = "历史", Credits = 4, }
          };

                foreach (var c in courses)
                    dbcontext.Courses.Add(c);
                dbcontext.SaveChanges();

                #endregion 课程种子数据

                #region 学生课程关联种子数据

                //这里学生的ID为 4、5、6是因为我们之前的种子数据中已经占了1、2、3 的id
                //所以新生成的id是4开头
                var studentCourses = new[] {
              new StudentCourse { CourseID = 1050, StudentID = 6, },
              new StudentCourse { CourseID = 4022, StudentID = 5, },
              new StudentCourse { CourseID = 2021, StudentID = 4, },
              new StudentCourse { CourseID = 4022, StudentID = 4, },
              new StudentCourse { CourseID = 2021, StudentID = 6, }
          };
                foreach (var sc in studentCourses)
                    dbcontext.StudentCourses.Add(sc);
                dbcontext.SaveChanges();

                #endregion 学生课程关联种子数据

                #region 用户种子数据

                if (dbcontext.Users.Any())
                {
                    return builder; // 数据已经初始化了。
                }
                var user = new ApplicationUser { Email = "ltm@ddxc.org", UserName = "ltm@ddxc.org", EmailConfirmed = true, City = "上海" };
                userManager.CreateAsync(user, "bb123456").Wait();// 等待异步方法执行完毕
                dbcontext.SaveChanges();
                var adminRole = "Admin";

                var role = new IdentityRole { Name = adminRole, };

                dbcontext.Roles.Add(role);
                dbcontext.SaveChanges();

                dbcontext.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
                dbcontext.SaveChanges();

                #endregion 用户种子数据
            }

            return builder;
        }
    }
}