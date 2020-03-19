using MockSchoolManagement.Application.Dtos;
using MockSchoolManagement.Models;
using System.Threading.Tasks;

namespace MockSchoolManagement.Application.Teachers
{
    public interface IDepartmentsService
    {
        /// <summary>
        /// 获取院系的分页信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<Department>> GetPagedDepartmentsList(GetDepartmentInput input);
    }
}