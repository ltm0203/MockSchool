using Microsoft.EntityFrameworkCore;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using MockSchoolManagement.Extensions;
using MockSchoolManagement.Application.Dtos;

namespace MockSchoolManagement.Application.Students.Dtos
{
    public class StudentService : IStudentService
    {

        private readonly IRepository<Student, int> _studentRepository;

        public StudentService(IRepository<Student, int> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<PagedResultDto<Student>> GetPaginatedResult(GetStudentInput input)
        {

            var query = _studentRepository.GetAll();
            //判断输入的搜索名称是否为空
            if (!string.IsNullOrEmpty(input.FilterText))
            {
                query = query.Where(s => s.Name.Contains(input.FilterText)
         || s.Email.Contains(input.FilterText));
            }
            //统计查询数据的总条数，用于分页计算总页数
            var count = query.Count();
            //根据需求进行排序，然后进行分页逻辑的计算
            query = query.OrderBy(input.Sorting).Skip((input.CurrentPage - 1)*input.MaxResultCount).Take(input.MaxResultCount);
            //将查询结果转换为List集合，加载到内存中
            var models = await query.AsNoTracking().ToListAsync();

            var dtos = new PagedResultDto<Student>
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
