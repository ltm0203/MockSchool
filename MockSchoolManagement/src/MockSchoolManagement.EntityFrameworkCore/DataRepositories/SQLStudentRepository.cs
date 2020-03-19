using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockSchoolManagement.EntityFrameworkCore;
using MockSchoolManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace MockSchoolManagement.DataRepositories
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly ILogger logger;
        private readonly AppDbContext _context;

        public SQLStudentRepository(AppDbContext context, ILogger<SQLStudentRepository> logger)
        {
            this.logger = logger;
            this._context = context;
        }

        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }


            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            logger.LogTrace("学生信息 Trace(跟踪) Log");
            logger.LogDebug("学生信息 Debug(调试) Log");
            logger.LogInformation("学生信息 信息(Information) Log");
            logger.LogWarning("学生信息 警告(Warning) Log");
            logger.LogError("学生信息 错误(Error) Log");
            logger.LogCritical("学生信息 严重(Critical) Log");
 
            return _context.Students;
        }

        public Student GetStudentById(int id)
        {

 

            return _context.Students.Find(id);
        }

        public Student Insert(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Update(Student updateStudent)
        {    
                var student = _context.Students.Attach(updateStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return updateStudent;
        }
    }
}