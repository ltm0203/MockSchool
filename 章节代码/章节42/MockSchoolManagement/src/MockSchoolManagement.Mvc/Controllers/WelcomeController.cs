using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;
using MockSchoolManagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly IRepository<Student, int> _studentRepository;

        public WelcomeController(IRepository<Student, int> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<string> Index()
        {
            var student = await _studentRepository.GetAll().FirstOrDefaultAsync();

            var longCount = await _studentRepository.LongCountAsync();

            return $" {student.Name}+{longCount} ";
        }

        public string Welcome()
        {
            return "我是Welcome控制器中的welcome操作方法";
        }
    }
}