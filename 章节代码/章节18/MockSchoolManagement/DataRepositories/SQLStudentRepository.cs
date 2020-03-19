using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Models;
using System.Collections.Generic;

namespace MockSchoolManagement.DataRepositories
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public SQLStudentRepository(AppDbContext context)
        {
            _context = context;
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