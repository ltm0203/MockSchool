using MockSchoolManagement.Models;
using MockSchoolManagement.Models.EnumTypes;
using System.Collections.Generic;
using System.Linq;

namespace MockSchoolManagement.DataRepositories
{
    public class MockStudentRepository : IStudentRepository
    {
        private readonly List<Student> _studentList;

        public MockStudentRepository()
        {
            _studentList = new List<Student>()
            {
            new Student() { Id = 1, Name = "张三", Major = MajorEnum.ComputerScience, Email = "zhangsan@52abp.com" },
            new Student() { Id = 2, Name = "李四", Major = MajorEnum.Mathematics, Email = "lisi@52abp.com" },
            new Student() { Id = 3, Name = "赵六", Major = MajorEnum.ElectronicCommerce, Email = "zhaoliu@52abp.com" },
            };
        }

        public Student Add(Student student)
        {
            student.Id = _studentList.Max(s => s.Id) + 1;
            _studentList.Add(student);
            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _studentList;
        }

        public Student GetStudent(int id)
        {
            return _studentList.FirstOrDefault(a => a.Id == id);
        }
    }
}