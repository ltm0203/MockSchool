using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "课程编号")]
        public int CourseID { get; set; }
        
        [Display(Name = "课程名称")]
        public string Title { get; set; }
        [Display(Name = "课程学分")]
        [Range(0, 5)]

        public int Credits { get; set; }
        [Display(Name = "院系")]

        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
         public virtual ICollection<CourseAssignment> CourseAssignments { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
