using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.Application.Courses.Dtos
{
    public class CourseCreateViewModel
    {
        [Display(Name ="课程编号")]
        public int CourseID { get; set; }
        [Display(Name = "课程名称")]

        public string Title { get; set; }
        [Display(Name = "课程学分")]

        public int Credits { get; set; }

    }
}
