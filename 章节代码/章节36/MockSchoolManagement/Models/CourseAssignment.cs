namespace MockSchoolManagement.Models
{

    /// <summary>
    /// 课程设置分配
    /// </summary>

    public class CourseAssignment
    {
        public int TeacherID { get; set; }
        public int CourseID { get; set; }
        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
    }
}