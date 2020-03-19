using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockSchoolManagement.Application.Courses;
using MockSchoolManagement.Application.Courses.Dtos;
using MockSchoolManagement.DataRepositories;
using MockSchoolManagement.Infrastructure;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;

namespace MockSchoolManagement.Controllers
{
    public class CourseController : Controller 
    {
        private readonly IRepository<Course, int> _courseRepository;
        private readonly ICourseService _courseService;

        public CourseController(IRepository<Course, int> courseRepository, ICourseService courseService)
        {
            _courseRepository = courseRepository;
            _courseService = courseService;
        }



        // 不填写 [HttpGet]默认为处理GET请求
        public async Task<ActionResult> Index(GetCourseInput input)
        {

      var models=      await _courseService.GetPaginatedResult(input);
            return View(models) ;
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseCreateViewModel input)
        {


            return View();
        }


    } 
         
    
}