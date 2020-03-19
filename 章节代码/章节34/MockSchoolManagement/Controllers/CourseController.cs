using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;

namespace MockSchoolManagement.Controllers
{
    public class CourseController : Controller
    {
        private readonly IRepository<Course, int> _courseRepository;

        public CourseController(IRepository<Course, int> courseRepository)
        {
            _courseRepository = courseRepository;
        }



        // 不填写 [HttpGet]默认为处理GET请求
        public async Task<ActionResult> Index()
        {
     var course=     await  _courseRepository.GetAllListAsync();




            return View();
        }

    } 
         
    
}