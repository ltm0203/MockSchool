namespace MockSchoolManagement.Models
{

    /// <summary>
    /// 课程设置分配
    /// </summary>

    public class CourseAssignment
    {
        public int TeacherID { get; set; }
        public int CourseID { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Course Course { get; set; }
    }
}