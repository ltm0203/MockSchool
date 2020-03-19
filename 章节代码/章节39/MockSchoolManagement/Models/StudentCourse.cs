using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.Models
{
    /// <summary>
    /// 成绩
    /// </summary>
    public enum Grade
    {
        A, B, C, D, F
    }

    public class StudentCourse
    {
        [Key]
        public int StudentCourseId { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText = "无成绩")]
        public Grade? Grade { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }

    }
}
