using MockSchoolManagement.Application.Dtos;
using MockSchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.Application.Teachers
{
    public interface ITeacherService
    {
        /// <summary>
        /// 获取老师的分页信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<Teacher>> GetPagedTeacherList(GetTeacherInput input);
        
    }
}
