using MockSchoolManagement.Application.Dtos;
using MockSchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.ViewModels
{
    public class TeacherListViewModel
    {


        public PagedResultDto<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        /// <summary>
        /// 选中的老师Id
        /// </summary>
        public int SelectedId { get; set; }
        /// <summary>
        /// 选中的课程Id
        /// </summary>
        public int SelectedCourseId { get; set; }

    }
}
