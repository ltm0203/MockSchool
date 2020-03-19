using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Models
{
    /// <summary>
    /// 课程实体
    /// </summary>
    public class Course
    {
        /// <summary>
        /// ID不允许自增
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }


        public ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
