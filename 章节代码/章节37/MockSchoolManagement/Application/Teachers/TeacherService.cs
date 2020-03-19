using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Application.Dtos;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace MockSchoolManagement.Application.Teachers
{
    public class TeacherService : ITeacherService
    {

        private readonly IRepository<Teacher, int> _teacherRepository;

        public TeacherService(IRepository<Teacher, int> teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<PagedResultDto<Teacher>> GetPagedTeacherList(GetTeacherInput input)
        {
            var query = _teacherRepository.GetAll();

            if (!string.IsNullOrEmpty(input.FilterText))
            {
                query = query.Where(s => s.Name.Contains(input.FilterText));
            }
            //统计查询数据的总条数，用于分页计算总页数
            var count = query.Count();
            //根据需求进行排序，然后进行分页逻辑的计算
            query = query.OrderBy(input.Sorting).Skip((input.CurrentPage - 1) * input.MaxResultCount).Take(input.MaxResultCount);
            //将查询结果转换为List集合，加载到内存中
         

         var models = await query.Include(a=>a.OfficeLocation) //加载导航属性OfficeLocation
                .Include(a=>a.CourseAssignments) //加载导航属性 CourseAssignments
                  .ThenInclude(a=>a.Course)//再加载CourseAssignments的导航属性Course
                  .ThenInclude(a=>a.StudentCourses)//再加载Course的导航属性 StudentCourses
                  .ThenInclude(a=>a.Student).//最后加载 StudentCourses的导航属性Student 课程关联的学生信息
                Include(i => i.CourseAssignments) //加载导航属性 CourseAssignments
                .ThenInclude(i => i.Course) //再加载CourseAssignments的导航属性Course
                 .ThenInclude(i => i.Department)  //最后加载Course 的导航属性 Department ，即课程关联在哪些院系
                .AsNoTracking().ToListAsync();
            var dtos = new PagedResultDto<Teacher>
            {
                TotalCount = count,
                CurrentPage = input.CurrentPage,
                MaxResultCount = input.MaxResultCount,
                Data = models,
                FilterText = input.FilterText,
                Sorting = input.Sorting
            };
            return dtos;
        }
    }
}
