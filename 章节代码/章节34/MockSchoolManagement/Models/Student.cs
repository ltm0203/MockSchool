using MockSchoolManagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
 
namespace MockSchoolManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
     /// <summary>
     /// 名字
     /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 主修科目
        /// </summary>
      
        public MajorEnum? Major { get; set; }
        
        public string Email { get; set; }
      
        public string PhotoPath { get; set; }


        [NotMapped]
        public string EncryptedId { get; set; }
        /// <summary>
        /// 入学时间
        /// </summary>
        public DateTime EnrollmentDate { get; set; }


        public ICollection<StudentCourse> StudentCourses { get; set; }


    }

    
}
